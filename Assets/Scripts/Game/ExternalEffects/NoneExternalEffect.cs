public sealed class NoneExternalEffect : IExternalEffect
{
    public NoneExternalEffect() { }

    public bool ShouldApply()
    {
        return false;
    }

    public void Apply() { }

    public void Reset() { }
}
