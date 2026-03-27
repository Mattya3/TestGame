using System.Collections;
using UnityEngine;
using static Constants;

[RequireComponent(typeof(AudioSource))]
public class EventMusicPlayer : MonoEventReactingBehaviour
{
    [SerializeField]
    private GameEvent _gameEvent;

    [SerializeField]
    private float _delayBeforePlaying = 1f;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    protected override bool _ShouldSubscribe(GameEvent gameEvent)
    {
        return gameEvent == _gameEvent;
    }

    protected override void OnFailure()
    {
        StartCoroutine(_CoPlay());
    }

    protected override void OnSuccess()
    {
        StartCoroutine(_CoPlay());
    }

    private IEnumerator _CoPlay()
    {
        yield return new WaitForSeconds(_delayBeforePlaying);
        _audioSource.Play();
    }
}
