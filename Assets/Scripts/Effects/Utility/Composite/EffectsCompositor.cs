using EffectsCompositeComponent;
using UnityEngine;
using UnityEngine.VFX;
using System.Collections;

public class EffectsCompositor : MonoBehaviour
{
    [SerializeField]
    private float _duration = 1f;

    [SerializeField]
    private float _delayTime = 0f;

    private VisualEffect _visualEffect;
    private ISoundEffect[] _soundEffects;
    private Coroutine _deactivateCoroutine;

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
            soundEffect.PlaySound();
        }
    }

    private IEnumerator _CoDeactivateAfterDuration()
    {
        yield return new WaitForSeconds(_duration);
        gameObject.SetActive(false);
        _deactivateCoroutine = null;
    }
}
