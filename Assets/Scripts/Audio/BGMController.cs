using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMController : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField, Min(0.0f)]
    private float _fadeOutTimeOnPlayerDied = 0.1f;

    [SerializeField, Min(0.0f)]
    private float _fadeOutTimeOnAllPlayersGoal = 1.0f;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.OnPlayerDied += _OnPlayerDied;
        GameManager.Instance.OnAllPlayersGoal += _OnAllPlayersGoal;
    }

    private void _OnPlayerDied()
    {
        StartCoroutine(_CoFadeOut(_fadeOutTimeOnPlayerDied));
    }

    private void _OnAllPlayersGoal()
    {
        StartCoroutine(_CoFadeOut(_fadeOutTimeOnAllPlayersGoal));
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
