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

    public void Initialize(
        IReadOnlyList<Player> players,
        IReadOnlyList<IExternalEffectContext> contexts
    )
    {
        InjectExternalEffect(players, contexts);
    }

    private void InjectExternalEffect(
        IReadOnlyList<Player> players,
        IReadOnlyList<IExternalEffectContext> contexts
    )
    {
        if (contexts == null)
            return;

        for (int i = 0; i < contexts.Count; i++)
        {
            Constants.ExternalEffectType effectType = GetExternalEffectType(i);
            IExternalEffect externalEffect = ExternalEffectFactory.Create(
                effectType,
                players,
                contexts[i]
            );
            contexts[i].SetExternalEffect(externalEffect);
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
