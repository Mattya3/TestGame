using UnityEngine.Rendering.Universal;

namespace EffectsCompositeComponent
{
    public interface ILightSourceEffect
    {
        void Initialize(Light2D light2D, bool playInUnscaledTime);

        void Play();
    }
}