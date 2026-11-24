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
        var target = new Player.PlayerExternalEffectTarget(player);

        switch (externalEffectType)
        {
            case Constants.ExternalEffectType.ReverseInput:
                return new ReverseInputExternalEffect(
                    playersAccess,
                    target
                );
            case Constants.ExternalEffectType.ReverseGravity:
                return new ReverseGravityExternalEffect(
                    playersAccess,
                    target
                );
            case Constants.ExternalEffectType.StopVerticalMovement:
                return new StopVerticalMovementExternalEffect(
                    playersAccess,
                    target
                );
            case Constants.ExternalEffectType.None:
            default:
                return new NoneExternalEffect();
        }
    }
}
