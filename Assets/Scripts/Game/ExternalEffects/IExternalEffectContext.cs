public interface IExternalEffectContext
{
    Player Player { get; }
    void SetExternalEffect(IExternalEffect externalEffect);
    void ResetExternalEffect();
}
