using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioLooper : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField, Min(1)]
    private uint _originalFrequency = 44100; // Unityがビルド時に周波数を変えてしまうことがあるため、元の周波数を外から指定

    [SerializeField]
    private uint _loopBeginSample = 0;

    [SerializeField]
    private uint _loopEndSample = 44100;

    private void Awake()
    {
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource is not assigned.");
            enabled = false;
            return;
        }
        if (_audioSource.clip == null)
        {
            Debug.LogError("AudioSource does not have an AudioClip assigned.");
            enabled = false;
            return;
        }

        if (_loopBeginSample >= _loopEndSample)
        {
            Debug.LogError("Loop begin sample must be less than loop end sample.");
            enabled = false;
            return;
        }
        if (_CorrectSample(_loopEndSample) > (uint)_audioSource.clip.samples)
        {
            Debug.LogError(
                "Loop end sample must be less than or equal to the total number of samples in the AudioClip."
            );
            enabled = false;
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_audioSource.isPlaying)
            return;

        // 再生中に周波数が変わることがある（らしい）ので、Updateで毎フレーム計算
        var correctedLoopEndSample = _CorrectSample(_loopEndSample);
        var correctedLoopDurationSamples =
            correctedLoopEndSample - _CorrectSample(_loopBeginSample);

        if (correctedLoopDurationSamples == 0)
            return; // ループ区間が無い場合は何もしない

        if (_audioSource.timeSamples >= correctedLoopEndSample)
        {
            _audioSource.timeSamples -= (int)correctedLoopDurationSamples;
        }
    }

    private uint _CorrectSample(uint sample)
    {
        // ビルド時に周波数が変わることがあるため、元の周波数を考慮してサンプル位置を補正
        return (uint)(sample * (ulong)_audioSource.clip.frequency / _originalFrequency);
    }
}
