namespace EffectsCompositeComponent
{
    public interface ICameraEffect
    {
        bool isEnabled { get; }
        void Initialize(CameraMutableAccess cameraAccess, bool playInUnscaledTime);
        void Play();
    }
}