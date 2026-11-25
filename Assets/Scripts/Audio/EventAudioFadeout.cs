using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[RequireComponent(typeof(AudioSource))]
public class EventAudioFadeout : MonoEventReactingBehaviour
{
    [Serializable]
    private struct EventFadeout
    {
        [SerializeField]
        private GameEvent _gameEvent;

        [SerializeField, Min(0.0f)]
        private float _fadeOutTime;

        public bool IsForEvent(GameEvent gameEvent) => _gameEvent == gameEvent;
        public float FadeOutTime => _fadeOutTime;
    }

    [SerializeField]
    private List<EventFadeout> _eventFadeouts;

    private AudioSource _audioSource;
    private Coroutine _fadeoutCoroutine;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    protected override void OnGameEvent(GameEvent gameEvent)
    {
        foreach (var eventFadeout in _eventFadeouts)
        {
            if (eventFadeout.IsForEvent(gameEvent))
            {
                if (_fadeoutCoroutine != null)
                    StopCoroutine(_fadeoutCoroutine);
                _fadeoutCoroutine = StartCoroutine(_CoFadeOut(eventFadeout.FadeOutTime));
                break;
            }
        }
    }

    private IEnumerator _CoFadeOut(float fadeOutTime)
    {
        var startVolume = _audioSource.volume;
        var elapsedTime = 0.0f;
        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            var t = Mathf.Clamp01(elapsedTime / fadeOutTime);
            _audioSource.volume = Mathf.Lerp(startVolume, 0.0f, t);
            yield return null;
        }
        _audioSource.volume = 0.0f;
        _audioSource.Stop();
        _audioSource.volume = startVolume; // 次回再生のために音量を元に戻す
        _fadeoutCoroutine = null;
    }
}
