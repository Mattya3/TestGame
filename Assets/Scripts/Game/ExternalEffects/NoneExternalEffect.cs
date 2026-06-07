using System.Collections.Generic;

public sealed class NoneExternalEffect : IExternalEffect
{
    public bool ShouldApply(IReadOnlyList<Player> players)
    {
        return false;
    }

    public void Apply(IExternalEffectContext context) { }

    public void Reset(IExternalEffectContext context) { }
}
