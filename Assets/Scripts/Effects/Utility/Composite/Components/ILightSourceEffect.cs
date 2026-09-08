namespace EffectsCompositeComponent
{
    public interface ILightSourceEffect
    {
        bool isEnabled { get; }
        void Initialize(bool playInUnscaledTime, float playSpeedRate);

        void Play();
    }
}