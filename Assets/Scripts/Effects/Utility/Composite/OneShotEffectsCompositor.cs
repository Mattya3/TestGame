using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OneShotEffectsCompositor : EffectsCompositorBase
{
    [SerializeField]
    private float _duration = 1f;

    [SerializeField]
    private float _delayTime = 0f;

    [SerializeField]
    private bool _playInUnscaledTime = false;

    private List<Coroutine> _playCoroutines = new List<Coroutine>();
    private Coroutine _deactivateCoroutine;

    public override void Initialize(
        AudioSource audioSource,
        CameraMutableAccess cameraAccess,
        TransformOffsetController transformOffsetController,
        Renderer renderer,
        Transform instantiationParent
        )
    {
        InitializeComponents(audioSource, cameraAccess, transformOffsetController, renderer, instantiationParent, _playInUnscaledTime);

        // 初期化時点では非アクティブにする
        gameObject.SetActive(false);
    }

    public override void PlayEffects()
    {
        gameObject.SetActive(true);

        _RemoveFinishedPlayCoroutines();
        _StopDeactivateCoroutine();

        _playCoroutines.Add(StartCoroutine(_CoPlayEffects()));
        _deactivateCoroutine = StartCoroutine(_CoDeactivateAfterDuration());
    }

    public override void StopEffects()
    {
        _StopAllPlayCoroutines();

        // この時点ではDeactivateしない。コルーチンによって一定時間後にDeactivateされるのを待機
    }

    private IEnumerator _CoPlayEffects()
    {
        yield return _playInUnscaledTime ? new WaitForSecondsRealtime(_delayTime) : new WaitForSeconds(_delayTime);
        PlayComponents();
    }

    private IEnumerator _CoDeactivateAfterDuration()
    {
        yield return _playInUnscaledTime ? new WaitForSecondsRealtime(_duration) : new WaitForSeconds(_duration);
        
        _StopAllPlayCoroutines();
        gameObject.SetActive(false);
        _deactivateCoroutine = null;
    }

    private void _StopAllPlayCoroutines()
    {
        foreach (var playCoroutine in _playCoroutines)
        {
            if (playCoroutine != null)
            {
                StopCoroutine(playCoroutine);
            }
        }
        _playCoroutines.Clear();
    }

    private void _StopDeactivateCoroutine()
    {
        if (_deactivateCoroutine != null)
        {
            StopCoroutine(_deactivateCoroutine);
            _deactivateCoroutine = null;
        }
    }

    private void _RemoveFinishedPlayCoroutines()
    {
        _playCoroutines.RemoveAll(coroutine => coroutine == null);
    }
}
