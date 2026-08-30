using UnityEngine;

namespace EffectsCompositeComponent
{
    public class SoundEffect : MonoBehaviour, ISoundEffect
    {
        [SerializeField]
        private AudioClip _audioClip;

        [SerializeField]
        private float _volume = 1.0f;

        private AudioSource _audioSource;

        private void Awake()
        {
            if (_audioClip == null)
            {
                Debug.LogWarning("No audio clip assigned to SoundEffect.");
            }
        }

        public void Initialize(AudioSource audioSource)
        {
            _audioSource = audioSource;
        }

        public void Play()
        {
            if (_audioClip == null || _audioSource == null)
            {
                Debug.LogWarning("Cannot play sound: No audio clip assigned or AudioSource is null.");
                return;
            }

            _audioSource.PlayOneShot(_audioClip, _volume);
        }
    }
}