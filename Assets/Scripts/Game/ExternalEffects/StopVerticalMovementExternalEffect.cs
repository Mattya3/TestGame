using System.Collections.Generic;

public sealed class StopVerticalMovementExternalEffect : IExternalEffect
{
    private readonly IReadOnlyList<Player> _players;
    private readonly IVerticalMovementEffectContext _context;

    public StopVerticalMovementExternalEffect(
        IReadOnlyList<Player> players,
        IVerticalMovementEffectContext context
    )
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
        _context.SetGravityScale(0f);
        _context.SetVerticalMovementStopped(true);
    }

    public void Reset()
    {
        _context.SetGravityScale(_context.GetDefaultGravityScale());
        _context.SetVerticalMovementStopped(false);
    }
}
