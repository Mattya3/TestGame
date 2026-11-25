using Unity.VisualScripting;
using UnityEngine;

namespace EffectsCompositeComponent
{
    public class RandomSoundEffect : MonoBehaviour, ISoundEffect
    {
        [SerializeField]
        private AudioClip[] _audioClips;

        [SerializeField]
        private SoundEffectOutput.Bus _outputBus = SoundEffectOutput.Bus.Action;

        [SerializeField, Range(0.0f, 1.0f)]
        private float _volume = 1.0f;

        [SerializeField]
        private bool _avoidImmediateRepeat = true;

        private SoundEffectOutput _output;
        private int _lastPlayedIndex = -1;

        public bool isEnabled => enabled;

        private void Awake()
        {
            if (_audioClips == null || _audioClips.Length == 0)
            {
                Debug.LogWarning("No audio clips assigned to RandomSoundEffect.");
                return;
            }
            for (int i = 0; i < _audioClips.Length; i++)
            {
                if (_audioClips[i] == null)
                {
                    Debug.LogWarning($"Audio clip at index {i} is null in RandomSoundEffect.");
                }
            }
        }

        public void Initialize(SoundEffectOutput soundEffectOutput)
        {
            _output = soundEffectOutput;

            if (_audioClips == null || _audioClips.Length == 0 || _output == null)
            {
                Debug.LogWarning(
                    "Cannot play sound: No audio clips assigned or SoundEffectOutput is null."
                );
                return;
            }
        }

        public void Play()
        {
            if (_audioClips == null || _audioClips.Length == 0 || _output == null)
                return;

            int nextIndex = _GetNextIndex();
            if (nextIndex != -1)
            {
                _output.PlayOneShot(_audioClips[nextIndex], _volume, _outputBus);
            }
        }

        private int _GetNextIndex()
        {
            if (_audioClips == null || _audioClips.Length == 0)
                return -1;

            if (_audioClips.Length == 1)
            {
                // If there is only one clip, always play it
                _lastPlayedIndex = 0;
                return _lastPlayedIndex;
            }

            if (_lastPlayedIndex == -1)
            {
                // If no clip has been played yet, select a random index
                _lastPlayedIndex = Random.Range(0, _audioClips.Length);
                return _lastPlayedIndex;
            }

            if (_avoidImmediateRepeat)
            {
                int randomIncrement = Random.Range(1, _audioClips.Length);
                int nextIndex = (_lastPlayedIndex + randomIncrement) % _audioClips.Length;
                _lastPlayedIndex = nextIndex;
                return nextIndex;
            }
            else
            {
                int nextIndex = Random.Range(0, _audioClips.Length);
                _lastPlayedIndex = nextIndex;
                return nextIndex;
            }
        }
    }
}
