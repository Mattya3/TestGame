using System.Collections.Generic;
using UnityEngine;

public sealed class ReverseInputExternalEffect : IExternalEffect
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

    public void Apply()
    {
        _target.RegisterInputDirectionConversion(_ConvertInputDirection);
    }

    public void Reset()
    {
        _target.RegisterInputDirectionConversion(null);
    }

    private Vector2 _ConvertInputDirection(Vector2 inputDirection)
    {
        return new Vector2(-inputDirection.x, inputDirection.y);
    }
}
