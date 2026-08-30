using EffectsCompositeComponent;
using UnityEngine;
using UnityEngine.VFX;

public class EffectsCompositor : MonoBehaviour
{
    [SerializeField]
    private float _duration = 1f;

    [SerializeField]
    private float _delayTime = 0f;

    private VisualEffect _visualEffect;
    private ISoundEffect[] _soundEffects;

    private void Awake()
    {
        _visualEffect = GetComponent<VisualEffect>();
        _soundEffects = GetComponents<ISoundEffect>();
    }

    public void Initialize(AudioSource audioSource)
    {
        foreach (var soundEffect in _soundEffects)
        {
            soundEffect.Initialize(audioSource);
        }
    }

    public void PlayEffects()
    {
        _visualEffect.Play();
        foreach (var soundEffect in _soundEffects)
        {
            soundEffect.PlaySound();
        }
    }

    private void Update()
    {
        
    }
}
