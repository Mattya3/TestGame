public abstract class UnplayableState : PlayerStateBase
{
    protected UnplayableState(IPlayerStateContext context)
        : base(context) { }

    public override void Freeze()
    {
        base.Freeze();
    }
}
