using UnityEngine;
using static Constants;

public class DeadState : UnplayableState
{
    private static readonly string _EFFECT_KEY_DEAD_ZONE = "DeadZoneDeath";
    private static readonly string _EFFECT_KEY_FALL = "FallDeath";

    private readonly DeathReason _deathReason;

    public DeadState(IPlayerStateContext context, PlayerSounds sounds, DeathReason deathReason)
        : base(context, sounds)
    {
        _deathReason = deathReason;
    }

    public override void OnEnabled()
    {
        Context.SetAnimatorUpdateMode(AnimatorUpdateMode.UnscaledTime);
        switch (_deathReason)
        {
            case DeathReason.Fall:
                Context.PlayCharacterEffect(_EFFECT_KEY_FALL);
                break;
            case DeathReason.DeadZone:
                Context.PlayCharacterEffect(_EFFECT_KEY_DEAD_ZONE);
                break;
        }

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

    public override string GetAnimationName()
    {
        return "Die";
    }

}
