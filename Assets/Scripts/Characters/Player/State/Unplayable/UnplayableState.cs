public abstract class UnplayableState : PlayerStateBase
{
    protected UnplayableState(IPlayerStateContext context, PlayerSounds sounds)
        : base(context, sounds) { }
}
