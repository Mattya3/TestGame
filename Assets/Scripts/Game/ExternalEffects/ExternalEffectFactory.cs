using System.Collections.Generic;

public static class ExternalEffectFactory
{
    public static IExternalEffect Create(
        Constants.ExternalEffectType externalEffectType,
        IReadOnlyList<Player> players,
        IExternalEffectContext context
    )
    {
        switch (externalEffectType)
        {
            case Constants.ExternalEffectType.None:
            case Constants.ExternalEffectType.ReverseInput:
            case Constants.ExternalEffectType.ReverseGravity:
            default:
                return new NoneExternalEffect(context);
        }
    }
}
