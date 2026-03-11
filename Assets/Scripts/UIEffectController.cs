using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UIEffectController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    private Action _onEffectComplete;

    void Awake()
    {
        if (_animator == null)
        {
            Debug.LogError("Animatorがアサインされていません。");
        }
    }

    /// <summary>
    /// 指定されたアニメーション演出（トリガー）を実行します。
    /// </summary>
    public void PlayDeathEffect(Action onComplete)
    {
        _PlayEffect(GameConstants.UI.FADE_BLACK_OUT, onComplete);
    }

    public void PlayGoalEffect(Action onComplete)
    {
        _PlayEffect(GameConstants.UI.FADE_BLACK_OUT, onComplete);
    }

    private void _PlayEffect(string triggerName, Action onComplete)
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
