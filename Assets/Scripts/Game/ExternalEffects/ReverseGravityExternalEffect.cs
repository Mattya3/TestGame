using System.Collections.Generic;

public sealed class ReverseGravityExternalEffect : IExternalEffect
{
    private readonly PlayersCollectionReadonlyAccess _players;
    private readonly IExternalEffectTarget _target;

    public ReverseGravityExternalEffect(PlayersCollectionReadonlyAccess playersAccess, IExternalEffectTarget target)
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
        _target.SetGravityScale(-_target.GetDefaultGravityScale());
    }

    public void Reset()
    {
        _target.SetGravityScale(_target.GetDefaultGravityScale());
    }
}
