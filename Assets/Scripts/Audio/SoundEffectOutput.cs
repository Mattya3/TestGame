using UnityEngine;

public class SoundEffectOutput : MonoBehaviour
{
    public enum Bus
    {
        System,
        Action,
        Feedback,
        Environment,
    }

    [SerializeField]
    private AudioSource _systemAudioSource;

    [SerializeField]
    private AudioSource _actionAudioSource;

    [SerializeField]
    private AudioSource _feedbackAudioSource;

    [SerializeField]
    private AudioSource _environmentAudioSource;

    public void PlayOneShot(AudioClip clip, float volume, Bus bus)
    {
        AudioSource audioSource = GetAudioSource(bus);
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }

    private AudioSource GetAudioSource(Bus bus)
    {
        switch (bus)
        {
            case Bus.System:
                return _systemAudioSource;
            case Bus.Action:
                return _actionAudioSource;
            case Bus.Feedback:
                return _feedbackAudioSource;
            case Bus.Environment:
                return _environmentAudioSource;
            default:
                return null;
        }
    }
}
