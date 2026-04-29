using static Constants;

public abstract class PlayableState : PlayerStateBase
{
    protected PlayableState(Player player, PlayerSounds sounds)
        : base(player, sounds) { }

    public override void Die(DeathReason deathReason)
    {
        Player.ChangeState(new DeadState(Player, Sounds));
        Player.NotifyDied(deathReason);
    }

    public override void Goal()
    {
        Player.ChangeState(new GoalState(Player, Sounds));
        Player.NotifyGoalReached();
    }
}
