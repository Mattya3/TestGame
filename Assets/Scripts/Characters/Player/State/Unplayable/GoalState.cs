using UnityEngine;
using static Constants;
using CharacterState;

public sealed class GoalState : CharacterUnplayableState
{
    public GoalState(ICharacterStateContext context, PlayerSounds sounds)
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
