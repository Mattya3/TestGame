using System.Collections.Generic;
using UnityEngine;

public class ExternalEffectManager : MonoBehaviour
{
    [SerializeField]
    private Constants.ExternalEffectType _externalEffectType = Constants.ExternalEffectType.None;

    private IExternalEffect _externalEffect;

    public void Initialize(IReadOnlyList<Player> players, IReadOnlyList<IExternalEffectContext> contexts)
    {
        _externalEffect = ExternalEffectFactory.Create(_externalEffectType, players);
        InjectExternalEffect(contexts);
    }

    private void InjectExternalEffect(IReadOnlyList<IExternalEffectContext> contexts)
    {
        if (contexts == null || _externalEffect == null)
            return;

        for (int i = 0; i < contexts.Count; i++)
        {
            contexts[i].SetExternalEffect(_externalEffect);
        }
    }
}
