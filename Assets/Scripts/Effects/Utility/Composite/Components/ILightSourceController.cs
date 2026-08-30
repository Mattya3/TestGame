using UnityEngine.Rendering.Universal;

namespace EffectsCompositeComponent
{
    public interface ILightSourceController
    {
        void Initialize(Light2D light2D, bool playInUnscaledTime);

        void Play();
    }
}