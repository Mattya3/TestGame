using System;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerSounds : MonoBehaviour
{
    [Serializable]
    private class AudioClipInfo
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField] private AudioSource _source;
        [SerializeField, Range(0.0f, 1.0f)] private float _volume = 1.0f;
    }

    [SerializeField] private AudioClipInfo _footstepSound;
    [SerializeField] private AudioClipInfo _jumpSound;
    [SerializeField] private AudioClipInfo _landSound;
    [SerializeField] private AudioClipInfo _deathSound;
    [SerializeField] private AudioClipInfo _goalSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
