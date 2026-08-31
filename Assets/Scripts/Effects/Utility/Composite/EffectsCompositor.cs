using EffectsCompositeComponent;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class EffectsCompositor : MonoBehaviour
{
    [SerializeField]
    private float _duration = 1f;

    [SerializeField]
    private float _delayTime = 0f;

    [SerializeField]
    private bool _playInUnscaledTime = false;

    private VisualEffect _visualEffect;
    private ISoundEffect[] _soundEffects;
    private ILightSourceEffect[] _lightSourceEffects;
    private ICameraEffect[] _cameraEffects;
    private ITransformEffect[] _transformEffects;
    private IRendererEffect[] _rendererEffects;

    private Coroutine _deactivateCoroutine;

    private void Awake()
    {
        _visualEffect = GetComponent<VisualEffect>();
        _soundEffects = GetComponents<ISoundEffect>();
        _lightSourceEffects = GetComponents<ILightSourceEffect>();
        _cameraEffects = GetComponents<ICameraEffect>();
        _transformEffects = GetComponents<ITransformEffect>();
        _rendererEffects = GetComponents<IRendererEffect>();
    }

    public void Initialize(
        AudioSource audioSource,
        CameraMutableAccess cameraAccess,
        TransformOffsetController transformOffsetController,
        Renderer renderer
        )
    {
        foreach (var soundEffect in _soundEffects)
        {
            soundEffect.Initialize(audioSource);
        }

        if (_lightSourceEffects.Length > 0)
        {
            var light2D = GetComponent<Light2D>();
            if (light2D == null)
            {
                Debug.LogWarning("Light2D component is missing. Light source effects will not be initialized.");
                _lightSourceEffects = new ILightSourceEffect[0]; // Clear the array to avoid further processing
            }
            foreach (var lightSourceEffect in _lightSourceEffects)
            {
                lightSourceEffect.Initialize(light2D, _playInUnscaledTime);
            }
        }

        foreach (var cameraEffect in _cameraEffects)
        {
            cameraEffect.Initialize(cameraAccess, _playInUnscaledTime);
        }

        foreach (var transformEffect in _transformEffects)
        {
            transformEffect.Initialize(transformOffsetController, _playInUnscaledTime);
        }

        foreach (var rendererEffect in _rendererEffects)
        {
            rendererEffect.Initialize(renderer, _playInUnscaledTime);
        }

        // 初期化時点では非アクティブにする
        gameObject.SetActive(false);
    }

    public void PlayEffects()
    {
        gameObject.SetActive(true);

        if (_deactivateCoroutine != null)
        {
            StopCoroutine(_deactivateCoroutine);
        }
        _deactivateCoroutine = StartCoroutine(_CoDeactivateAfterDuration());

        StartCoroutine(_CoPlayEffects());
    }

    private IEnumerator _CoPlayEffects()
    {
        yield return _playInUnscaledTime ? new WaitForSecondsRealtime(_delayTime) : new WaitForSeconds(_delayTime);

        if (_visualEffect.enabled)
            _visualEffect.Play();
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

    private IEnumerator _CoDeactivateAfterDuration()
    {
        yield return _playInUnscaledTime ? new WaitForSecondsRealtime(_duration) : new WaitForSeconds(_duration);
        gameObject.SetActive(false);
        _deactivateCoroutine = null;
    }
}
