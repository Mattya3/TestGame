using EffectsCompositeComponent;
using UnityEngine;
using UnityEngine.VFX;

public abstract class EffectsCompositorBase : MonoBehaviour, IEffectsCompositor
{
    private VisualEffect _visualEffect;
    private ISoundEffect[] _soundEffects;
    private ILightSourceEffect[] _lightSourceEffects;
    private ICameraEffect[] _cameraEffects;
    private ITransformEffect[] _transformEffects;
    private IRendererEffect[] _rendererEffects;

    protected virtual void Awake()
    {
        _visualEffect = GetComponent<VisualEffect>();
        _soundEffects = GetComponents<ISoundEffect>();
        _lightSourceEffects = GetComponents<ILightSourceEffect>();
        _cameraEffects = GetComponents<ICameraEffect>();
        _transformEffects = GetComponents<ITransformEffect>();
        _rendererEffects = GetComponents<IRendererEffect>();
    }

    protected void InitializeComponents(
        AudioSource audioSource,
        CameraMutableAccess cameraAccess,
        TransformOffsetController transformOffsetController,
        Renderer renderer,
        bool playInUnscaledTime
    )
    {
        foreach (var soundEffect in _soundEffects)
        {
            soundEffect.Initialize(audioSource);
        }
        foreach (var lightSourceEffect in _lightSourceEffects)
        {
            lightSourceEffect.Initialize(playInUnscaledTime);
        }
        foreach (var cameraEffect in _cameraEffects)
        {
            cameraEffect.Initialize(cameraAccess, playInUnscaledTime);
        }
        foreach (var transformEffect in _transformEffects)
        {
            transformEffect.Initialize(transformOffsetController, playInUnscaledTime);
        }
        foreach (var rendererEffect in _rendererEffects)
        {
            rendererEffect.Initialize(renderer, playInUnscaledTime);
        }
    }

    protected void PlayComponents()
    {
        if (_visualEffect != null)
        {
            _visualEffect.Play();
        }
        foreach (var soundEffect in _soundEffects)
        {
            if (soundEffect.isEnabled)
                soundEffect.Play();
        }
        foreach (var lightSourceEffect in _lightSourceEffects)
        {
            if (lightSourceEffect.isEnabled)
                lightSourceEffect.Play();
        }
        foreach (var cameraEffect in _cameraEffects)
        {
            if (cameraEffect.isEnabled)
                cameraEffect.Play();
        }
        foreach (var transformEffect in _transformEffects)
        {
            if (transformEffect.isEnabled)
                transformEffect.Play();
        }
        foreach (var rendererEffect in _rendererEffects)
        {
            if (rendererEffect.isEnabled)
                rendererEffect.Play();
        }
    }

    public abstract void Initialize(AudioSource audioSource, CameraMutableAccess cameraAccess, TransformOffsetController transformOffsetController, Renderer renderer);
    public abstract void PlayEffects();
    public abstract void StopEffects();
}
