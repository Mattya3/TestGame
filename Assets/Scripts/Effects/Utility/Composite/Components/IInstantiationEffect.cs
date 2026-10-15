using UnityEngine;

namespace EffectsCompositeComponent
{
    public interface IInstantiationEffect
    {
        bool isEnabled { get; }
        void Initialize(
            Transform instantiationParent,
            bool playInUnscaledTime,
            float playbackSpeedRate
        );

        void Play();
    }
}
