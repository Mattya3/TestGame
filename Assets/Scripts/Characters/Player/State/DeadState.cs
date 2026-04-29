using UnityEngine;
using static Constants;

public class DeadState : UnplayableState
{
    public DeadState(IPlayerStateContext context, PlayerSounds sounds)
        : base(context, sounds) { }

    public override void OnEnabled()
    {
        Context.Freeze();
        Sounds.OnDeath();
    }

    public override void Die(DeathReason deathReason)
    {
        Debug.LogWarning("Die() was called while in DeadState.", Context.LogContext);
    }

    public override void Goal()
    {
        Debug.LogWarning("Goal() was called while in DeadState.", Context.LogContext);
    }
}
