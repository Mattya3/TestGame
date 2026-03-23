using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EventMusicPlayer : MonoBehaviour
{
    [SerializeField]
    private GameConstants.GameEvent _targetEvent;

    [SerializeField]
    private float _delayBeforePlaying = 1f;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.RegisterEvent(_targetEvent, _Play);
    }

    private void _Play()
    {
        StartCoroutine(_CoPlay());
    }

    private IEnumerator _CoPlay()
    {
        yield return new WaitForSeconds(_delayBeforePlaying);
        _audioSource.Play();
    }
}
