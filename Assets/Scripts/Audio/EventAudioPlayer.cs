using System.Collections;
using UnityEngine;
using static Constants;

[RequireComponent(typeof(AudioSource))]
public class EventAudioPlayer : MonoEventReactingBehaviour
{
    [SerializeField]
    private GameEvent _gameEvent;

    [SerializeField]
    private float _delayBeforePlaying = 1f;

    private AudioSource _audioSource;
    private Coroutine _playCoroutine;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    protected override void OnGameEvent(GameEvent gameEvent)
    {
        if (gameEvent == _gameEvent)
        {
            if (_playCoroutine != null)
                StopCoroutine(_playCoroutine);
            _playCoroutine = StartCoroutine(_CoPlay());
        }
    }

    private IEnumerator _CoPlay()
    {
        yield return new WaitForSecondsRealtime(_delayBeforePlaying);
        _audioSource.Play();
        _playCoroutine = null;
    }
}
