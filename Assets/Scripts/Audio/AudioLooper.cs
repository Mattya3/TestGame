using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioLooper : MonoBehaviour
{
    [SerializeField]
    private AudioLoop _audioLoop;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioLoop == null )
        {
            Debug.LogError("AudioLoop is not assigned.");
            enabled = false;
            return;
        }
        if (!_audioLoop.Validate())
        {
            enabled = false;
            return;
        }

        _audioSource.clip = _audioLoop.Clip;
        _audioSource.Play();
    }

    void Update()
    {
        if (!_audioSource.isPlaying)
            return;

        // 再生中に周波数が変わることがある（らしい）ので、Updateで毎フレーム計算
        var correctedLoopBeginSample = _audioLoop.CorrectedBeginSample((uint)_audioSource.clip.frequency);
        var correctedLoopEndSample = _audioLoop.CorrectedEndSample((uint)_audioSource.clip.frequency);
        var correctedLoopDurationSamples = correctedLoopEndSample - correctedLoopBeginSample;

        if (correctedLoopDurationSamples == 0)
            return; // ループ区間が無い場合は何もしない

        if (_audioSource.timeSamples >= correctedLoopEndSample)
        {
            _audioSource.timeSamples -= (int)correctedLoopDurationSamples;
        }
    }
}
