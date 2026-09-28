using System;
using CharacterState;
using UnityEngine;

[Serializable]
public class PlayerSounds : CharacterSounds
{
    [Serializable]
    private class AudioClipInfo
    {
        [SerializeField]
        private AudioClip _clip;

        [SerializeField]
        private AudioSource _source;

        [SerializeField, Range(0.0f, 1.0f)]
        private float _volume = 1.0f;

        public bool IsValid(string soundName)
        {
            if (_clip == null)
            {
                Debug.LogError($"AudioClip for {soundName} is null.");
                return false;
            }
            if (_source == null)
            {
                Debug.LogError($"AudioSource for {soundName} is null.");
                return false;
            }
            return true;
        }

        public void Play()
        {
            _source.PlayOneShot(_clip, _volume);
        }
    }

    [SerializeField]
    private AudioClipInfo _goalSound;

    public override bool IsValid()
    {
        return base.IsValid() && _goalSound.IsValid("Goal");
    }

    public void OnGoal() => _goalSound.Play();
}
