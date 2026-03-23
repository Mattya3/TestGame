using System;
using UnityEngine;
using UnityEngine.Audio;

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

        public bool IsValid()
        {
            if (_clip == null)
            {
                Debug.LogError("AudioClip is null.");
                return false;
            }
            if (_source == null)
            {
                Debug.LogError("AudioSource is null.");
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
            !_footstepSound.IsValid()
            || !_jumpSound.IsValid()
            || !_landSound.IsValid()
            || !_deathSound.IsValid()
            || !_goalSound.IsValid()
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
