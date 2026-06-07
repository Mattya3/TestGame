public sealed class NoneExternalEffect : IExternalEffect
{
    public NoneExternalEffect(IExternalEffectContext context) { }

    public bool ShouldApply()
    {
        return false;
    }

    public void Apply() { }

    public void Reset() { }
}
