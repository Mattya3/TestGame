using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(StageSceneContextReadonlyAccess))]
[RequireComponent(typeof(GameManagerMutableAccess))]
public class ScreenEffectsController : MonoEventReactingBehaviour
{
    [Serializable]
    private struct EffectSettings
    {
        [HideInInspector]
        public readonly string _triggerName;
        public AnimatorUpdateMode _updateMode;

        public EffectSettings(string triggerName, AnimatorUpdateMode updateMode)
        {
            _triggerName = triggerName;
            _updateMode = updateMode;
        }
    }

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private EffectSettings _openingEffect = new EffectSettings(
        Constants.AnimationTrigger.OPENING,
        AnimatorUpdateMode.Normal
    );

    [SerializeField]
    private EffectSettings _restartEffect = new EffectSettings(
        Constants.AnimationTrigger.RESTART,
        AnimatorUpdateMode.Normal
    );

    [SerializeField]
    private EffectSettings _failureEffect = new EffectSettings(
        Constants.AnimationTrigger.FAILURE,
        AnimatorUpdateMode.UnscaledTime
    );

    [SerializeField]
    private EffectSettings _successEffect = new EffectSettings(
        Constants.AnimationTrigger.SUCCESS,
        AnimatorUpdateMode.UnscaledTime
    );

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
            _PlayEffect(_restartEffect);
        else
            _PlayEffect(_openingEffect);
    }

    protected override void OnSuccess()
    {
        _PlayEffect(_successEffect);
    }

    protected override void OnFailure()
    {
        _PlayEffect(_failureEffect);
    }

    private void _PlayEffect(EffectSettings settings)
    {
        _animator.updateMode = settings._updateMode;
        _animator.SetTrigger(settings._triggerName);
    }

    public void OnOpeningEffectComplete()
    {
        _gameManagerAccess.HandlePlayStart();
    }

    public void OnClosingEffectComplete()
    {
        _gameManagerAccess.HandleSceneEnd();
    }
}
