using static Constants;

public abstract class PlayableState : PlayerStateBase
{
    protected PlayableState(Player player)
        : base(player) { }

    public override void Die(DeathReason deathReason)
    {
        Player.ChangeState(new DeadState(Player));
        Player.NotifyDied(deathReason);
    }

    public override void Goal()
    {
        Player.ChangeState(new GoalState(Player));
        Player.NotifyGoalReached();
    }
}
