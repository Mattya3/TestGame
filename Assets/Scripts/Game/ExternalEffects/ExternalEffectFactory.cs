using System.Collections.Generic;
using UnityEngine;

public static class ExternalEffectFactory
{
    public static IExternalEffect Create(
        Constants.ExternalEffectType externalEffectType,
        PlayersCollectionReadonlyAccess playersAccess,
        Player player
    )
    {
        switch (externalEffectType)
        {
            case Constants.ExternalEffectType.ReverseInput:
                return new ReverseInputExternalEffect(
                    playersAccess,
                    new PlayerInputDirectionEffectTarget()
                );
            case Constants.ExternalEffectType.ReverseGravity:
                return new ReverseGravityExternalEffect(
                    playersAccess,
                    new PlayerGravityEffectTarget(player.GetComponent<Rigidbody2D>())
                );
            case Constants.ExternalEffectType.StopVerticalMovement:
                return new StopVerticalMovementExternalEffect(
                    playersAccess,
                    new PlayerVerticalMovementEffectTarget(player.GetComponent<Rigidbody2D>())
                );
            case Constants.ExternalEffectType.None:
            default:
                return new NoneExternalEffect();
        }
    }
}
