using EffectsCompositeComponent;
using UnityEngine;
using UnityEngine.VFX;

public abstract class EffectsCompositorBase : MonoBehaviour, IEffectsCompositor
{
    private VisualEffect[] _visualEffects;
    private ISoundEffect[] _soundEffects;
    private ILightSourceEffect[] _lightSourceEffects;
    private ICameraEffect[] _cameraEffects;
    private ITransformEffect[] _transformEffects;
    private IRendererEffect[] _rendererEffects;
    private IInstantiationEffect[] _instantiationEffects;

    protected virtual void Awake()
    {
        _visualEffects = GetComponents<VisualEffect>();
        _soundEffects = GetComponents<ISoundEffect>();
        _lightSourceEffects = GetComponents<ILightSourceEffect>();
        _cameraEffects = GetComponents<ICameraEffect>();
        _transformEffects = GetComponents<ITransformEffect>();
        _rendererEffects = GetComponents<IRendererEffect>();
        _instantiationEffects = GetComponents<IInstantiationEffect>();
    }

    protected void InitializeComponents(
        AudioSource audioSource,
        CameraMutableAccess cameraAccess,
        TransformOffsetController transformOffsetController,
        Renderer renderer,
        Transform instantiationParent,
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
        foreach (var instantiationEffect in _instantiationEffects)
        {
            instantiationEffect.Initialize(instantiationParent, playInUnscaledTime);
        }
    }

    protected void PlayComponents()
    {
        foreach (var visualEffect in _visualEffects)
        {
            if (visualEffect.enabled)
                visualEffect.Play();
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
        foreach (var instantiationEffect in _instantiationEffects)
        {
            if (instantiationEffect.isEnabled)
                instantiationEffect.Play();
        }
    }

    public abstract void Initialize(AudioSource audioSource, CameraMutableAccess cameraAccess, TransformOffsetController transformOffsetController, Renderer renderer, Transform instantiationParent);
    public abstract void PlayEffects();
    public abstract void StopEffects();
}
