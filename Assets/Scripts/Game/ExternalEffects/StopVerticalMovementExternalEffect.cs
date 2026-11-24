using System.Collections.Generic;

public sealed class StopVerticalMovementExternalEffect : IExternalEffect
{
    private readonly PlayersCollectionReadonlyAccess _players;
    private readonly IVerticalMovementEffectTarget _target;

    public StopVerticalMovementExternalEffect(
        PlayersCollectionReadonlyAccess playersAccess,
        IVerticalMovementEffectTarget target
    )
    {
        _players = playersAccess;
        _target = target;
    }

    public bool ShouldApply()
    {
        return ExternalEffectCondition.AreAllPlayersInputtingHorizontal(_players);
    }

    public void Apply()
    {
        _target.SetGravityScale(0f);
        _target.SetVerticalMovementStopped(true);
    }

    public void Reset()
    {
        _target.SetGravityScale(_target.GetDefaultGravityScale());
        _target.SetVerticalMovementStopped(false);
    }
}
