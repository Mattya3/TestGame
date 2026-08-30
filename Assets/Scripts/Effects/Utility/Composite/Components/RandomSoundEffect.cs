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
        }

        public void PlaySound()
        {
            if (_audioClips.Length == 0 || _audioSource == null)
            {
                Debug.LogWarning("Cannot play sound: No audio clips assigned or AudioSource is null.");
                return;
            }

            int randomIndex = Random.Range(0, _audioClips.Length);
            _audioSource.PlayOneShot(_audioClips[randomIndex], _volume);
        }
    }
}