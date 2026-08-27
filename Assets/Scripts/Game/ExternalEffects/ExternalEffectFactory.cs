using System.Collections.Generic;
using UnityEngine;

public static class ExternalEffectFactory
{
    public static IExternalEffect Create(
        Constants.ExternalEffectType externalEffectType,
        IReadOnlyList<Player> players,
        int playerIndex,
        IExternalEffectContext context
    )
    {
        Player player = players[playerIndex];

        switch (externalEffectType)
        {
            case Constants.ExternalEffectType.ReverseInput:
                return new ReverseInputExternalEffect(
                    players,
                    new PlayerInputDirectionEffectContext()
                );
            case Constants.ExternalEffectType.ReverseGravity:
                return new ReverseGravityExternalEffect(
                    players,
                    new PlayerGravityEffectContext(player.GetComponent<Rigidbody2D>())
                );
            case Constants.ExternalEffectType.StopVerticalMovement:
                return new StopVerticalMovementExternalEffect(
                    players,
                    new PlayerVerticalMovementEffectContext(player.GetComponent<Rigidbody2D>())
                );
            case Constants.ExternalEffectType.None:
            default:
                return new NoneExternalEffect(context);
        }
    }
}
