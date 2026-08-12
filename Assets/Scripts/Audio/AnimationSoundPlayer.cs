using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class AnimationSoundPlayer : MonoBehaviour
{
    // アニメーションイベントでは引数を1つしか渡せないため、音量スケールをシリアライズ
    [SerializeField]
    private float _volumeScale = 1.0f;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(Object clipObject)
    {
        var clip = clipObject as AudioClip;
        if (clip == null)
            return;

        _audioSource.PlayOneShot(clip, _volumeScale);
    }
}
