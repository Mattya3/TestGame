using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UIEffectController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    private Action _onEffectComplete;

    /// <summary>
    /// 指定されたアニメーション演出（トリガー）を実行します。
    /// </summary>
    public void PlayEffect(string triggerName, Action onComplete)
    {
        _onEffectComplete = onComplete;
        gameObject.SetActive(true);
        _animator.SetTrigger(triggerName);
    }

    public void OnEffectComplete()
    {
        _onEffectComplete?.Invoke();
        _onEffectComplete = null;
    }
}
