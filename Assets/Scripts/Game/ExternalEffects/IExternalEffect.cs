public interface IExternalEffect
{
    bool ShouldApply();
    void Apply();
    void Reset();
}
