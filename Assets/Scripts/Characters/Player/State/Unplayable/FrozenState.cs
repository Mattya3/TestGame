using UnityEngine;
using static Constants;

public class FrozenState : UnplayableState
{
    public FrozenState(IPlayerStateContext context)
        : base(context) { }

    public override void OnEnabled()
    {
        Context.SetAnimatorSpeed(0f);
        Context.Freeze();
    }

    public override void Die(DeathReason deathReason)
    {
        Debug.LogWarning("Die() was called while in FrozenState.", Context.LogContext);
    }

    public override void Goal()
    {
        Debug.LogWarning("Goal() was called while in FrozenState.", Context.LogContext);
    }
}
