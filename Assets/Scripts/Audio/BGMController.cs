using System.Collections;
using UnityEngine;
using static Constants;

[RequireComponent(typeof(AudioSource))]
public class BGMController : MonoEventReactingBehaviour
{
    [SerializeField, Min(0.0f)]
    private float _fadeOutTimeOnFailure = 0.1f;

    [SerializeField, Min(0.0f)]
    private float _fadeOutTimeOnSuccess = 1.0f;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    protected override void OnFailure()
    {
        StartCoroutine(_CoFadeOut(_fadeOutTimeOnFailure));
    }

    protected override void OnSuccess()
    {
        StartCoroutine(_CoFadeOut(_fadeOutTimeOnSuccess));
    }

    private IEnumerator _CoFadeOut(float fadeOutTime)
    {
        var startVolume = _audioSource.volume;
        var elapsedTime = 0.0f;
        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            var t = Mathf.Clamp01(elapsedTime / fadeOutTime);
            _audioSource.volume = Mathf.Lerp(startVolume, 0.0f, t);
            yield return null;
        }
        _audioSource.volume = 0.0f;
        _audioSource.Stop();
        _audioSource.volume = startVolume; // 次回再生のために音量を元に戻す
    }
}
