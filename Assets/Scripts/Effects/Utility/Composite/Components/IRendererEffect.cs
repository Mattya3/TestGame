using UnityEngine;

namespace EffectsCompositeComponent
{
    public interface IRendererEffect
    {
        bool isEnabled { get; }
        void Initialize(Renderer renderer, bool playInUnscaledTime);
        void Play();
    }
}
