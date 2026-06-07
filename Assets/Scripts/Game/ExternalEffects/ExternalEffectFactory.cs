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
                return new NoneExternalEffect(context);
            case Constants.ExternalEffectType.ReverseInput:
                if (context is IInputDirectionEffectContext inputDirectionContext)
                {
                    return new ReverseInputExternalEffect(players, inputDirectionContext);
                }
                return new NoneExternalEffect(context);
            case Constants.ExternalEffectType.ReverseGravity:
                if (context is IGravityEffectContext gravityContext)
                {
                    return new ReverseGravityExternalEffect(players, gravityContext);
                }
                return new NoneExternalEffect(context);
            default:
                return new NoneExternalEffect(context);
        }
    }
}
