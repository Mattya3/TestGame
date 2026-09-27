using UnityEngine;
using static Constants;

public class DeadState : UnplayableState
{
    private readonly DeathReason _deathReason;

    public DeadState(IPlayerStateContext context, PlayerSounds sounds, DeathReason deathReason)
        : base(context, sounds)
    {
        _deathReason = deathReason;
    }

    public override void OnEnabled()
    {
        Context.NotifyDied(_deathReason);
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
