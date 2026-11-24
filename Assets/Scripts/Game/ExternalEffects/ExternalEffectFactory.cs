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
                    new Player.PlayerExternalEffectTarget(player)
                );
            case Constants.ExternalEffectType.ReverseGravity:
                return new ReverseGravityExternalEffect(
                    playersAccess,
                    new Player.PlayerExternalEffectTarget(player)
                );
            case Constants.ExternalEffectType.StopVerticalMovement:
                return new StopVerticalMovementExternalEffect(
                    playersAccess,
                    new Player.PlayerExternalEffectTarget(player)
                );
            case Constants.ExternalEffectType.None:
            default:
                return new NoneExternalEffect();
        }
    }
}
