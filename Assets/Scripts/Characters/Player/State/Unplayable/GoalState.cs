using UnityEngine;
using static Constants;
using CharacterState;

public sealed class GoalState : CharacterUnplayableState
{
    private readonly PlayerSounds _playerSounds;

    public GoalState(ICharacterStateContext context, PlayerSounds sounds)
        : base(context, sounds)
    {
        _playerSounds = sounds;
    }

    public override void OnEnabled()
    {
        Context.NotifyGoalReached();
        Context.Freeze();
        _playerSounds.OnGoal();
    }

    public override void Die(DeathReason deathReason)
    {
        Debug.LogWarning("Die() was called while in GoalState.", Context.LogContext);
    }
}
