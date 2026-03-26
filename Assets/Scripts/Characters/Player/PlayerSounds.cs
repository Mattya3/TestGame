using System;
using UnityEngine;

[Serializable]
public class PlayerSounds
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
    private AudioClipInfo _footstepSound;

    [SerializeField]
    private AudioClipInfo _jumpSound;

    [SerializeField]
    private AudioClipInfo _landSound;

    [SerializeField]
    private AudioClipInfo _deathSound;

    [SerializeField]
    private AudioClipInfo _goalSound;

    public bool IsValid()
    {
        if (
            !_footstepSound.IsValid("Footstep")
            || !_jumpSound.IsValid("Jump")
            || !_landSound.IsValid("Land")
            || !_deathSound.IsValid("Death")
            || !_goalSound.IsValid("Goal")
        )
        {
            return false;
        }
        return true;
    }

    public void OnFootstep() => _footstepSound.Play();

    public void OnJump() => _jumpSound.Play();

    public void OnLand() => _landSound.Play();

    public void OnDeath() => _deathSound.Play();

    public void OnGoal() => _goalSound.Play();
}
