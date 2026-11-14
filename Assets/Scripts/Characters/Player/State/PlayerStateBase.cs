using UnityEngine;
using static Constants;

public abstract class PlayerStateBase : IPlayerState
{
    protected PlayerStateBase(IPlayerStateContext context)
    {
        Context = context;
    }

    protected IPlayerStateContext Context { get; }

    public virtual void OnMove(Vector2 inputDirection) { }

    public virtual void OnJump() { }

    public virtual void Die(DeathReason deathReason) { }

    public virtual void Goal() { }

    public virtual void OnEnabled() { }

    public virtual void OnDisabled() { }

    public virtual string GetAnimationName()
    {
        return AnimationTransitions.IGNORED_ANIMATION_NAME;
    }
}
