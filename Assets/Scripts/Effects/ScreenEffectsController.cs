using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ScreenEffectsController : MonoBehaviour, IScreenEffects
{
    [SerializeField]
    private Animator _animator;

    private Action _onEffectComplete;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        // シーン開始時にLocatorに登録
        ScreenEffectsLocator.Provide(this);
    }

    private void OnDestroy()
    {
        // シーン終了時にLocatorから登録解除
        ScreenEffectsLocator.Clear();
    }

    public void PlayOpeningEffect(Action onComplete)
    {
        _PlayEffect(Constants.AnimationTrigger.OPENING, onComplete);
    }

    /// <summary>
    /// 指定されたアニメーション演出（トリガー）を実行します。
    /// </summary>
    public void PlayFailureEffect(Action onComplete)
    {
        _PlayEffect(Constants.AnimationTrigger.FAILURE, onComplete);
    }

    public void PlaySuccessEffect(Action onComplete)
    {
        _PlayEffect(Constants.AnimationTrigger.SUCCESS, onComplete);
    }

    private void _PlayEffect(string triggerName, Action onComplete)
    {
        _onEffectComplete = onComplete;
        _animator.SetTrigger(triggerName);
    }

    public void OnEffectComplete()
    {
        _onEffectComplete?.Invoke();
        _onEffectComplete = null;
    }
}
