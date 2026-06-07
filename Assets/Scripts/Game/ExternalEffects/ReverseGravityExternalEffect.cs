using System.Collections.Generic;

public sealed class ReverseGravityExternalEffect : IExternalEffect
{
    private readonly IReadOnlyList<Player> _players;
    private readonly IGravityEffectContext _context;

    public ReverseGravityExternalEffect(IReadOnlyList<Player> players, IGravityEffectContext context)
    {
        _players = players;
        _context = context;
    }

    public bool ShouldApply()
    {
        return ExternalEffectCondition.AreAllPlayersInputtingHorizontal(_players);
    }

    public void Apply()
    {
        _context.SetGravityScale(-_context.GetDefaultGravityScale());
    }

    public void Reset()
    {
        _context.SetGravityScale(_context.GetDefaultGravityScale());
    }
}
