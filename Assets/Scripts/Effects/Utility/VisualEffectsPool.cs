using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public static class VisualEffectsPool
{
    public enum EffectPlayMode
    {
        Instantiate,
        Persistent,
    }

    public interface IEffectPlayer
    {
        void Play(MonoBehaviour owner);
        void Cleanup(MonoBehaviour owner);
    }

    [System.Serializable]
    public struct EffectConfig
    {
        [SerializeField]
        private EffectPlayMode _playMode;

        [SerializeField]
        private GameObject _effectPrefab;

        [SerializeField]
        private Vector3 _position;

        [SerializeField]
        private float _duration;

        [SerializeField]
        private string _vfxEventName;

        [SerializeField]
        private AudioClip _audioClip;

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField, Range(0.0f, 1.0f)]
        private float _audioVolume;

        public IEffectPlayer CreateEffectPlayer(Transform parent)
        {
            if (_effectPrefab == null)
                return null;

            switch (_playMode)
            {
                case EffectPlayMode.Instantiate:
                    return new InstantiateEffectPlayer(_effectPrefab, _position, _duration, _audioClip, _audioSource, _audioVolume, parent);
                case EffectPlayMode.Persistent:
                    return new PersistentEffectPlayer(_effectPrefab, _position, _vfxEventName, _audioClip, _audioSource, _audioVolume, parent);
                default:
                    Debug.LogError($"Unhandled EffectPlayMode value: {_playMode}");
                    return null;
            }
        }
    }

    private abstract class EffectPlayerBase : IEffectPlayer
    {
        protected readonly GameObject _prefab;
        protected readonly Vector3 _position;
        protected readonly Transform _parent;
        protected readonly AudioClip _audioClip;
        protected readonly AudioSource _audioSource;
        protected readonly float _audioVolume;

        protected EffectPlayerBase(GameObject prefab, Vector3 position, AudioClip audioClip, AudioSource audioSource, float audioVolume, Transform parent)
        {
            _prefab = prefab;
            _position = position;
            _audioClip = audioClip;
            _audioSource = audioSource;
            _audioVolume = audioVolume;
            _parent = parent;
        }

        public abstract void Play(MonoBehaviour owner);
        public abstract void Cleanup(MonoBehaviour owner);

        protected void PlayAudio()
        {
            if (_audioClip != null && _audioSource != null)
            {
                _audioSource.PlayOneShot(_audioClip, _audioVolume);
            }
        }
    }

    private class InstantiateEffectPlayer : EffectPlayerBase
    {
        private readonly float _duration;
        private readonly List<GameObject> _activeInstances;
        private readonly List<Coroutine> _activeCoroutines;

        public InstantiateEffectPlayer(GameObject prefab, Vector3 position, float duration, AudioClip audioClip, AudioSource audioSource, float audioVolume, Transform parent)
            : base(prefab, position, audioClip, audioSource, audioVolume, parent)
        {
            _duration = duration;
            _activeInstances = new List<GameObject>();
            _activeCoroutines = new List<Coroutine>();
        }

        public override void Play(MonoBehaviour owner)
        {
            GameObject instance = Object.Instantiate(_prefab, _position, Quaternion.identity, _parent);
            _activeInstances.Add(instance);
            PlayAudio();

            Coroutine coroutine = owner.StartCoroutine(_CoDestroyAfterDuration(instance, owner));
            _activeCoroutines.Add(coroutine);
        }

        private IEnumerator _CoDestroyAfterDuration(GameObject instance, MonoBehaviour owner)
        {
            yield return new WaitForSeconds(_duration);

            int index = _activeInstances.IndexOf(instance);
            if (index >= 0)
            {
                _activeInstances.RemoveAt(index);
                _activeCoroutines.RemoveAt(index);
            }

            if (instance != null)
            {
                Object.Destroy(instance);
            }
        }

        public override void Cleanup(MonoBehaviour owner)
        {
            for (int i = 0; i < _activeCoroutines.Count; i++)
            {
                if (_activeCoroutines[i] != null)
                {
                    owner.StopCoroutine(_activeCoroutines[i]);
                }
            }

            for (int i = 0; i < _activeInstances.Count; i++)
            {
                if (_activeInstances[i] != null)
                {
                    Object.Destroy(_activeInstances[i]);
                }
            }

            _activeInstances.Clear();
            _activeCoroutines.Clear();
        }
    }

    private class PersistentEffectPlayer : EffectPlayerBase
    {
        private readonly string _vfxEventName;

        private GameObject _instance;
        private VisualEffect _visualEffect;
        private VFXEventAttribute _eventAttribute;

        public PersistentEffectPlayer(GameObject prefab, Vector3 position, string vfxEventName, AudioClip audioClip, AudioSource audioSource, float audioVolume, Transform parent)
            : base(prefab, position, audioClip, audioSource, audioVolume, parent)
        {
            _vfxEventName = vfxEventName;
            _Initialize();
        }

        private void _Initialize()
        {
            _instance = Object.Instantiate(_prefab, _position, Quaternion.identity, _parent);
            _visualEffect = _instance.GetComponent<VisualEffect>();

            if (_visualEffect == null)
            {
                Debug.LogError($"VisualEffect component not found on {_prefab.name}");
                Object.Destroy(_instance);
                _instance = null;
                return;
            }

            _eventAttribute = _visualEffect.CreateVFXEventAttribute();
        }

        public override void Play(MonoBehaviour owner)
        {
            if (_visualEffect == null)
                return;

            _visualEffect.SendEvent(_vfxEventName, _eventAttribute);
            PlayAudio();
        }

        public override void Cleanup(MonoBehaviour owner)
        {
            if (_instance != null)
            {
                Object.Destroy(_instance);
                _instance = null;
            }

            _visualEffect = null;
            _eventAttribute = null;
        }
    }
}
