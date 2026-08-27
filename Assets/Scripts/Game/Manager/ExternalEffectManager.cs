using System.Collections.Generic;
using UnityEngine;

public class ExternalEffectManager : MonoBehaviour
{
    [SerializeField]
    private Constants.ExternalEffectType _player1ExternalEffectType = Constants
        .ExternalEffectType
        .None;

    [SerializeField]
    private Constants.ExternalEffectType _player2ExternalEffectType = Constants
        .ExternalEffectType
        .None;

        if (players == null)
        {
            Debug.LogError("players が null です。", this);
            return;
        }
        for (int i = 0; i < players.Count; i++)
        {
            Player player = players[i];
            if (player == null)
                continue;

            IExternalEffectContext context = player.ExternalEffectContext;
            Constants.ExternalEffectType effectType = GetExternalEffectType(i);
            IExternalEffect externalEffect = ExternalEffectFactory.Create(
                effectType,
                players,
                i,
                context
            );
            context.SetExternalEffect(externalEffect);
        }
    }

    private Constants.ExternalEffectType GetExternalEffectType(int playerIndex)
    {
        if (playerIndex == 0)
            return _player1ExternalEffectType;
        if (playerIndex == 1)
            return _player2ExternalEffectType;

        return Constants.ExternalEffectType.None;
    }
}
