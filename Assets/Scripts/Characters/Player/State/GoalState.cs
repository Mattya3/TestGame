using UnityEngine;
using static Constants;

public class GoalState : UnplayableState
{
    public GoalState(Player player, PlayerSounds sounds)
        : base(player, sounds) { }

    public override void OnEnabled()
    {
        Player.Freeze();
        Sounds.OnGoal();
    }

    public override void Die(DeathReason deathReason)
    {
        Debug.LogWarning("Die() was called while in GoalState.", Player);
    }
}
