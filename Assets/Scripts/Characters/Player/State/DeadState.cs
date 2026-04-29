using UnityEngine;
using static Constants;

public class DeadState : UnplayableState
{
    public DeadState(Player player, PlayerSounds sounds)
        : base(player, sounds) { }

    public override void OnEnabled()
    {
        Player.Freeze();
        Sounds.OnDeath();
    }

    public override void Die(DeathReason deathReason)
    {
        Debug.LogWarning("Die() was called while in DeadState.", Player);
    }

    public override void Goal()
    {
        Debug.LogWarning("Goal() was called while in DeadState.", Player);
    }
}
