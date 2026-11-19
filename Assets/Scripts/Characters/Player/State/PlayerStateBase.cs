using UnityEngine;
using static Constants;

public abstract class PlayerStateBase : IPlayerState
{
    private float _elapsedTimeInState = 0f;
    private float _elapsedUnscaledTimeInState = 0f;

    protected PlayerStateBase(IPlayerStateContext context)
    {
        Context = context;
    }

    protected IPlayerStateContext Context { get; }

    protected float ElapsedTimeInState => _elapsedTimeInState;
    protected float ElapsedUnscaledTimeInState => _elapsedUnscaledTimeInState;

    public virtual void OnMove(Vector2 inputDirection)
    {
        _elapsedTimeInState += Time.deltaTime;
        _elapsedUnscaledTimeInState += Time.unscaledDeltaTime;
    }

    public virtual void OnJump() { }

    public virtual void Die(DeathReason deathReason) { }

    public virtual void Goal() { }

    public virtual void OnEnabled()
    {
        _elapsedTimeInState = 0f;
        _elapsedUnscaledTimeInState = 0f;
    }

    public virtual void OnDisabled() { }

    public virtual string GetAnimationName()
    {
        return AnimationTransitions.IGNORED_ANIMATION_NAME;
    }
}
