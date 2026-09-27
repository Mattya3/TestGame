using System.Collections.Generic;
using UnityEngine;

public static class ExternalEffectFactory
{
    public static IExternalEffect Create(
        Constants.ExternalEffectType externalEffectType,
        IReadOnlyList<Player> players,
        int playerIndex
    )
    {
        Player player = players[playerIndex];

        switch (externalEffectType)
        {
            case Constants.ExternalEffectType.ReverseInput:
                return new ReverseInputExternalEffect(
                    players,
                    new PlayerInputDirectionEffectTarget()
                );
            case Constants.ExternalEffectType.ReverseGravity:
                return new ReverseGravityExternalEffect(
                    players,
                    new PlayerGravityEffectTarget(player.GetComponent<Rigidbody2D>())
                );
            case Constants.ExternalEffectType.StopVerticalMovement:
                return new StopVerticalMovementExternalEffect(
                    players,
                    new PlayerVerticalMovementEffectTarget(player.GetComponent<Rigidbody2D>())
                );
            case Constants.ExternalEffectType.None:
            default:
                return new NoneExternalEffect();
        }
    }
}
