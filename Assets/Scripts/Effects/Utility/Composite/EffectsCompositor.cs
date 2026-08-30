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

    private VisualEffect _visualEffect;
    private ISoundEffect[] _soundEffects;
    private ILightSourceController[] _lightSourceControllers;
    private Coroutine _deactivateCoroutine;

    private void Awake()
    {
        _visualEffect = GetComponent<VisualEffect>();
        _soundEffects = GetComponents<ISoundEffect>();
        _lightSourceControllers = GetComponents<ILightSourceController>();
    }

    public void Initialize(AudioSource audioSource)
    {
        foreach (var soundEffect in _soundEffects)
        {
            soundEffect.Initialize(audioSource);
        }

        if (_lightSourceControllers.Length > 0)
        {
            var light2D = GetComponent<Light2D>();
            if (light2D == null)
            {
                Debug.LogWarning("Light2D component is missing. Light source controllers will not be initialized.");
                _lightSourceControllers = new ILightSourceController[0]; // Clear the array to avoid further processing
            }
            foreach (var lightSourceController in _lightSourceControllers)
            {
                lightSourceController.Initialize(light2D);
            }
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
        yield return new WaitForSeconds(_delayTime);

        _visualEffect.Play();
        foreach (var soundEffect in _soundEffects)
        {
            soundEffect.Play();
        }
        foreach (var lightSourceController in _lightSourceControllers)
        {
            lightSourceController.Play();
        }
    }

    private IEnumerator _CoDeactivateAfterDuration()
    {
        yield return new WaitForSeconds(_duration);
        gameObject.SetActive(false);
        _deactivateCoroutine = null;
    }
}
