using System.Collections.Generic;

public sealed class ReverseGravityExternalEffect : IExternalEffect
{
    private readonly IReadOnlyList<Player> _players;
    private readonly IGravityEffectTarget _target;

    public ReverseGravityExternalEffect(IReadOnlyList<Player> players, IGravityEffectTarget target)
    {
        _players = players;
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
