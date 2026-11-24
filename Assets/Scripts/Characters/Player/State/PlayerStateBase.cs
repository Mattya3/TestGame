using UnityEngine;
using static Constants;

public abstract class PlayerStateBase : IPlayerState
{
    private float _enabledTime = 0f;
    private float _enabledUnscaledTime = 0f;

    protected PlayerStateBase(IPlayerStateContext context)
    {
        Context = context;

        _enabledTime = Time.time;
        _enabledUnscaledTime = Time.unscaledTime;
    }

    protected IPlayerStateContext Context { get; }

    protected float ElapsedTimeInState => Time.time - _enabledTime;
    protected float ElapsedUnscaledTimeInState => Time.unscaledTime - _enabledUnscaledTime;

    public virtual void Update(Vector2 inputDirection) { }

    public virtual void FixedUpdate(Vector2 inputDirection) { }

    public virtual void OnJump() { }

    public virtual void Die(DeathReason deathReason) { }

    public virtual void Goal() { }

    public virtual void OnEnabled()
    {
        _enabledTime = Time.time;
        _enabledUnscaledTime = Time.unscaledTime;
    }

    public virtual void OnDisabled() { }

    public virtual bool IsAlive => true;
    public virtual bool HasReachedGoal => false;
    public virtual string GetAnimationName()
    {
        return AnimationTransitions.IGNORED_ANIMATION_NAME;
    }
}
