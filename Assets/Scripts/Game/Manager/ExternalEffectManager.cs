using System.Collections.Generic;
using UnityEngine;

public class ExternalEffectManager : MonoBehaviour
{
    [SerializeField]
    private Constants.ExternalEffectType _externalEffectType = Constants.ExternalEffectType.None;

    [SerializeField]
    private List<Constants.ExternalEffectType> _externalEffectTypesByPlayer = new();

    public void Initialize(IReadOnlyList<Player> players, IReadOnlyList<IExternalEffectContext> contexts)
    {
        InjectExternalEffect(players, contexts);
    }

    private void InjectExternalEffect(IReadOnlyList<Player> players, IReadOnlyList<IExternalEffectContext> contexts)
    {
        if (contexts == null)
            return;

        for (int i = 0; i < contexts.Count; i++)
        {
            Constants.ExternalEffectType effectType = GetExternalEffectType(i);
            IExternalEffect externalEffect = ExternalEffectFactory.Create(effectType, players, contexts[i]);
            contexts[i].SetExternalEffect(externalEffect);
        }
    }

    private Constants.ExternalEffectType GetExternalEffectType(int playerIndex)
    {
        if (_externalEffectTypesByPlayer == null || playerIndex < 0)
            return _externalEffectType;
        if (playerIndex >= _externalEffectTypesByPlayer.Count)
            return _externalEffectType;

        return _externalEffectTypesByPlayer[playerIndex];
    }
}
