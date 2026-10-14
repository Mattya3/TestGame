using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotEffectsCompositor : EffectsCompositorBase
{
    [SerializeField]
    private float _duration = 1f;

    [SerializeField]
    private float _delayTime = 0f;

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
        InitializeComponents(
            audioSource,
            cameraAccess,
            transformOffsetController,
            renderer,
            instantiationParent
        );

        // 初期化時点では非アクティブにする
        gameObject.SetActive(false);
    }

    public override void PlayEffects()
    {
        gameObject.SetActive(true);

        _StopDeactivateCoroutine();

        Coroutine playCoroutine = null;
        playCoroutine = StartCoroutine(_CoPlayEffects(() => _playCoroutines.Remove(playCoroutine)));
        _playCoroutines.Add(playCoroutine);
        _deactivateCoroutine = StartCoroutine(_CoDeactivateAfterDuration());
    }

    public override void StopEffects()
    {
        _StopAllPlayCoroutines();

        // この時点ではDeactivateしない。コルーチンによって一定時間後にDeactivateされるのを待機
    }

    private IEnumerator _CoPlayEffects(Action onFinished)
    {
        try
        {
            yield return PlayInUnscaledTime
                ? new WaitForSecondsRealtime(_delayTime)
                : new WaitForSeconds(_delayTime);
            PlayComponents();
        }
        finally
        {
            onFinished?.Invoke();
        }
    }

    private IEnumerator _CoDeactivateAfterDuration()
    {
        yield return PlayInUnscaledTime
            ? new WaitForSecondsRealtime(_duration)
            : new WaitForSeconds(_duration);

        _StopAllPlayCoroutines();
        gameObject.SetActive(false);
        _deactivateCoroutine = null;
    }

    private void _StopAllPlayCoroutines()
    {
        var playCoroutines = _playCoroutines.ToArray();
        _playCoroutines.Clear();

        foreach (var playCoroutine in playCoroutines)
        {
            if (playCoroutine != null)
            {
                StopCoroutine(playCoroutine);
            }
        }
    }

    private void _StopDeactivateCoroutine()
    {
        if (_deactivateCoroutine != null)
        {
            StopCoroutine(_deactivateCoroutine);
            _deactivateCoroutine = null;
        }
    }
}
