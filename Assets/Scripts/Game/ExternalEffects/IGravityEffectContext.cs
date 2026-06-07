public interface IGravityEffectContext : IExternalEffectContext
{
    float GravityScale { get; }
    void SetGravityScale(float gravityScale);
}
