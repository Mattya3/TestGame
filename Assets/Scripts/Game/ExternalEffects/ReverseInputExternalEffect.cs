using System.Collections.Generic;
using UnityEngine;

public sealed class ReverseInputExternalEffect : IExternalEffect, IInputDirectionEffect
{
    private readonly PlayersCollectionReadonlyAccess _players;
    private readonly IExternalEffectTarget _target;

    public ReverseInputExternalEffect(
        PlayersCollectionReadonlyAccess playersAccess,
        IExternalEffectTarget target
    )
    {
        _players = playersAccess;
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
