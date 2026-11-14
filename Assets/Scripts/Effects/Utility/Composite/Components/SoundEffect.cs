using UnityEngine;

namespace EffectsCompositeComponent
{
    public class SoundEffect : MonoBehaviour, ISoundEffect
    {
        [SerializeField]
        private AudioClip _audioClip;

        [SerializeField]
        private SoundEffectOutput.Bus _outputBus = SoundEffectOutput.Bus.Action;

        [SerializeField, Range(0.0f, 1.0f)]
        private float _volume = 1.0f;

        private SoundEffectOutput _output;

        public bool isEnabled => enabled;

        private void Awake()
        {
            if (_audioClip == null)
            {
                Debug.LogWarning("No audio clip assigned to SoundEffect.");
            }
        }

        public void Initialize(SoundEffectOutput soundEffectOutput)
        {
            _output = soundEffectOutput;

            if (_audioClip == null || _output == null)
            {
                Debug.LogWarning(
                    "Cannot play sound: No audio clip assigned or SoundEffectOutput is null."
                );
                return;
            }
        }

        public void Play()
        {
            if (_audioClip == null || _output == null)
                return;

            _output.PlayOneShot(_audioClip, _volume, _outputBus);
        }
    }
}
