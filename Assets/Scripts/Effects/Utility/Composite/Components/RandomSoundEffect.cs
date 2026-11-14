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

        private SoundEffectOutput _output;

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

            int randomIndex = Random.Range(0, _audioClips.Length);
            _output.PlayOneShot(_audioClips[randomIndex], _volume, _outputBus);
        }
    }
}
