using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(StageSceneContextReadonlyAccess))]
[RequireComponent(typeof(GameManagerMutableAccess))]
public class ScreenEffectsController : MonoEventReactingBehaviour
{
    [SerializeField]
    private Animator _animator;

    private StageSceneContextReadonlyAccess _stageContextAccess;
    private GameManagerMutableAccess _gameManagerAccess;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _stageContextAccess = GetComponent<StageSceneContextReadonlyAccess>();
        _gameManagerAccess = GetComponent<GameManagerMutableAccess>();
    }

    private void Start()
    {
        if (_stageContextAccess.AfterRestart)
            _PlayRestartEffect();
        else
            _PlayOpeningEffect();
    }

    protected override void OnSuccess()
    {
        _PlaySuccessEffect();
    }

    protected override void OnFailure()
    {
        _PlayFailureEffect();
    }

    private void _PlayOpeningEffect()
    {
        _PlayEffect(Constants.AnimationTrigger.OPENING);
    }

    private void _PlayRestartEffect()
    {
        _PlayEffect(Constants.AnimationTrigger.RESTART);
    }

    private void _PlayFailureEffect()
    {
        _PlayEffect(Constants.AnimationTrigger.FAILURE);
    }

    private void _PlaySuccessEffect()
    {
        _PlayEffect(Constants.AnimationTrigger.SUCCESS);
    }

    private void _PlayEffect(string triggerName)
    {
        _animator.SetTrigger(triggerName);
    }

    public void OnOpeningEffectComplete()
    {
        // TODO: 演出完了後の処理（例: プレイヤーの操作を許可するなど）をここに実装
    }

    public void OnClosingEffectComplete()
    {
        _gameManagerAccess.HandleSceneEnd();
    }
}
