using System.Collections.Generic;
using UnityEngine;

public sealed class ReverseInputExternalEffect : IExternalEffect, IInputDirectionEffect
{
    private readonly IReadOnlyList<Player> _players;
    private readonly IInputDirectionEffectTarget _target;

    public ReverseInputExternalEffect(
        IReadOnlyList<Player> players,
        IInputDirectionEffectTarget target
    )
    {
        _players = players;
        _target = target;
    }

    public bool ShouldApply()
    {
        return ExternalEffectCondition.AreAllPlayersInputtingHorizontal(_players);
    }

    public void Apply() { }

    public void Reset() { }

    public Vector2 ConvertInputDirection(Vector2 inputDirection)
    {
        return _target.ReverseHorizontalInput(inputDirection);
    }
}
