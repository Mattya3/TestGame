using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EventMusicPlayer : MonoBehaviour
{
    [SerializeField]
    private float _delayBeforePlaying = 1f;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        StartCoroutine(_CoPlay());
    }

    private IEnumerator _CoPlay()
    {
        yield return new WaitForSeconds(_delayBeforePlaying);
        _audioSource.Play();
    }
}
