using System.Collections.Generic;
using UnityEngine;

public sealed class ReverseInputExternalEffect : IExternalEffect, IInputDirectionEffect
{
    private readonly IReadOnlyList<Player> _players;
    private readonly IInputDirectionEffectContext _context;

    public ReverseInputExternalEffect(IReadOnlyList<Player> players, IInputDirectionEffectContext context)
    {
        _players = players;
        _context = context;
    }

    public bool ShouldApply()
    {
        return ExternalEffectCondition.AreAllPlayersInputtingHorizontal(_players);
    }

    public void Apply() { }

    public void Reset() { }

    public Vector2 ConvertInputDirection(Vector2 inputDirection)
    {
        return _context.ReverseHorizontalInput(inputDirection);
    }
}
