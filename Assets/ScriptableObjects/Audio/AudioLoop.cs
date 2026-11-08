using UnityEngine;

[CreateAssetMenu(fileName = "AudioLoop", menuName = "Scriptable Objects/Audio/AudioLoop")]
public class AudioLoop : ScriptableObject
{
    private const uint DEFAULT_SAMPLE_RATE = 44100;

    [SerializeField, Min(1)]
    private uint _originalFrequency = DEFAULT_SAMPLE_RATE; // Unityがビルド時に周波数を変えてしまうことがあるため、元の周波数を外から指定

    [SerializeField]
    private uint _loopBeginSample = 0;

    [SerializeField]
    private uint _loopEndSample = DEFAULT_SAMPLE_RATE;

    [SerializeField]
    private AudioClip _clip;

    public bool Validate()
    {
        if (_clip == null)
        {
            Debug.LogError("AudioClip is not assigned.");
            return false;
        }
        if (_loopBeginSample >= _loopEndSample)
        {
            Debug.LogError("Loop begin sample must be less than loop end sample.");
            return false;
        }
        if (CorrectedEndSample((uint)_clip.frequency) > (uint)_clip.samples)
        {
            Debug.LogError(
                "Loop end sample must be less than or equal to the total number of samples in the AudioClip."
            );
            return false;
        }
        return true;
    }

    public uint CorrectedBeginSample(uint currentPlaybackFrequency)
    {
        return _CorrectSample(_loopBeginSample, currentPlaybackFrequency);
    }

    public uint CorrectedEndSample(uint currentPlaybackFrequency)
    {
        return _CorrectSample(_loopEndSample, currentPlaybackFrequency);
    }

    private uint _CorrectSample(uint sample, uint currentPlaybackFrequency)
    {
        return (uint)(sample * (ulong)currentPlaybackFrequency / _originalFrequency);
    }

    public AudioClip Clip => _clip;
}
