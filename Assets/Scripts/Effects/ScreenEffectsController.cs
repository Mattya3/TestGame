using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ScreenEffectsController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    private Action _onEffectComplete;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
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
