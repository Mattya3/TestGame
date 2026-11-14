using UnityEngine;
using static Constants;

public class GoalState : UnplayableState
{
    private static readonly string _EFFECT_KEY_GOAL = "Goal";

    public GoalState(IPlayerStateContext context, PlayerSounds sounds)
        : base(context, sounds) { }

    public override void OnEnabled()
    {
        Context.PlayCharacterEffect(_EFFECT_KEY_GOAL);
        Context.NotifyGoalReached();
        Context.Freeze();
    }

    public override void Die(DeathReason deathReason)
    {
        Debug.LogWarning("Die() was called while in GoalState.", Context.LogContext);
    }
}
