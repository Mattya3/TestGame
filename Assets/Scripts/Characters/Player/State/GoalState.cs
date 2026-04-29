using UnityEngine;
using static Constants;

public class GoalState : UnplayableState
{
    public GoalState(Player player)
        : base(player) { }

    public override void OnEnabled()
    {
        Player.Freeze();
        Player.PlayGoalSound();
    }

    public override void Die(DeathReason deathReason)
    {
        Debug.LogWarning("Die() was called while in GoalState.", Player);
    }
}
