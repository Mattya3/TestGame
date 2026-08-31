using UnityEngine;

namespace EffectsCompositeComponent
{
    public class RandomSoundEffect : MonoBehaviour, ISoundEffect
    {
        [SerializeField]
        private AudioClip[] _audioClips;

        [SerializeField]
        private float _volume = 1.0f;

        private AudioSource _audioSource;

        public bool isEnabled => enabled;

        private void Awake()
        {
            if (_audioClips == null || _audioClips.Length == 0)
            {
                Debug.LogWarning("No audio clips assigned to RandomSoundEffect.");
            }
            for (int i = 0; i < _audioClips.Length; i++)
            {
                if (_audioClips[i] == null)
                {
                    Debug.LogWarning($"Audio clip at index {i} is null in RandomSoundEffect.");
                }
            }
        }

        public void Initialize(AudioSource audioSource)
        {
            _audioSource = audioSource;

            if (_audioClips.Length == 0 || _audioSource == null)
            {
                Debug.LogWarning("Cannot play sound: No audio clips assigned or AudioSource is null.");
                return;
            }
        }

        public void Play()
        {
            if (_audioClips.Length == 0 || _audioSource == null)
                return;

            int randomIndex = Random.Range(0, _audioClips.Length);
            _audioSource.PlayOneShot(_audioClips[randomIndex], _volume);
        }
    }
}