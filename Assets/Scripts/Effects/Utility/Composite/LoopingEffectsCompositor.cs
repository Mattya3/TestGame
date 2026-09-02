using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LoopingEffectsCompositor : EffectsCompositorBase
{
    [SerializeField]
    private float _loopTime = 1f;

    [SerializeField]
    private float _fadeOutTime = 0.5f;

    private bool _isPlaying = false;
    private float _replayTimer = 0f;
    private Coroutine _deactivateCoroutine;

    public override void Initialize(
        AudioSource audioSource,
        CameraMutableAccess cameraAccess,
        TransformOffsetController transformOffsetController,
        Renderer renderer,
        Transform instantiationParent
        )
    {
        InitializeComponents(audioSource, cameraAccess, transformOffsetController, renderer, instantiationParent);

        // 初期化時点では非アクティブにする
        gameObject.SetActive(false);
    }

    public override void PlayEffects()
    {
        gameObject.SetActive(true);

        _StopDeactivateCoroutine();

        _isPlaying = true;
        _replayTimer = _loopTime;

        PlayComponents();
    }

    public override void StopEffects()
    {
        _isPlaying = false;
        StartCoroutine(_CoDeactivateAfterFadeout());
    }

    protected override void Update()
    {
        base.Update();

        // loopTime経過ごとにPlayComponents()を実行

        if (!_isPlaying)
            return;

        _replayTimer -= PlayInUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        if (_replayTimer > 0f)
            return;

        _replayTimer += _loopTime;
        PlayComponents();
    }

    private IEnumerator _CoDeactivateAfterFadeout()
    {
        yield return PlayInUnscaledTime ? new WaitForSecondsRealtime(_fadeOutTime) : new WaitForSeconds(_fadeOutTime);

        gameObject.SetActive(false);
        _deactivateCoroutine = null;
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
