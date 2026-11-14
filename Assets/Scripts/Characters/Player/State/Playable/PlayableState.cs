using static Constants;

public abstract class PlayableState : PlayerStateBase
{
    protected PlayableState(IPlayerStateContext context)
        : base(context) { }

    public override void Die(DeathReason deathReason)
    {
        Context.ChangeState(new DeadState(Context, deathReason));
    }

    public override void Goal()
    {
        Context.ChangeState(new GoalState(Context));
    }
}
