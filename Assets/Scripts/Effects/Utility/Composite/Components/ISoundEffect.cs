using UnityEngine;

namespace EffectsCompositeComponent
{
    public interface ISoundEffect
    {
        bool isEnabled { get; }
        void Initialize(SoundEffectOutput soundEffectOutput);
        void Play();
    }
}
