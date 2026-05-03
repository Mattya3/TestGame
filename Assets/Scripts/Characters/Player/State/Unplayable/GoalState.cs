using UnityEngine;
using static Constants;

public class GoalState : UnplayableState
{
    public GoalState(IPlayerStateContext context, PlayerSounds sounds)
        : base(context, sounds) { }

    public override void OnEnabled()
    {
        Context.NotifyGoalReached();
        Context.Freeze();
        Sounds.OnGoal();
    }

    public override void Die(DeathReason deathReason)
    {
        Debug.LogWarning("Die() was called while in GoalState.", Context.LogContext);
    }
}
