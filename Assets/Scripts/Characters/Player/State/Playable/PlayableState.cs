using static Constants;

public abstract class PlayableState : PlayerStateBase
{
    protected PlayableState(IPlayerStateContext context, PlayerSounds sounds)
        : base(context, sounds) { }

    public override void Die(DeathReason deathReason)
    {
        Context.ChangeState(new DeadState(Context, Sounds));
    }

    public override void Goal()
    {
        Context.ChangeState(new GoalState(Context, Sounds));
    }
}
