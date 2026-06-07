using System.Collections.Generic;

public interface IExternalEffect
{
    bool ShouldApply(IReadOnlyList<Player> players);
    void Apply(IExternalEffectContext context);
    void Reset(IExternalEffectContext context);
}
