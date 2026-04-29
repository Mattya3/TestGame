using UnityEngine;
using static Constants;

public class FrozenState : UnplayableState
{
    public FrozenState(Player player)
        : base(player) { }

    public override void OnEnabled()
    {
        Player.Freeze();
    }

    public override void Die(DeathReason deathReason)
    {
        Debug.LogWarning("Die() was called while in FrozenState.", Player);
    }

    public override void Goal()
    {
        Debug.LogWarning("Goal() was called while in FrozenState.", Player);
    }
}
