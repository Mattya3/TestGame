using UnityEngine;

namespace EffectsCompositeComponent
{
    public interface ITransformEffect
    {
        bool isEnabled { get; }
        void Initialize(TransformOffsetController transformOffsetController, bool playInUnscaledTime);
        void Play();
    }

}