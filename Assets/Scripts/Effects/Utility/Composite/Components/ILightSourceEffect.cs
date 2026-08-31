using UnityEngine.Rendering.Universal;

namespace EffectsCompositeComponent
{
    public interface ILightSourceEffect
    {
        bool isEnabled { get; }
        void Initialize(Light2D light2D, bool playInUnscaledTime);

        void Play();
    }
}