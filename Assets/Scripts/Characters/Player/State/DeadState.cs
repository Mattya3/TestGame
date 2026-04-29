using UnityEngine;
using static Constants;

public class DeadState : UnplayableState
{
    public DeadState(Player player)
        : base(player) { }

    public override void OnEnabled()
    {
        Player.Freeze();
        Player.PlayDeathSound();
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
