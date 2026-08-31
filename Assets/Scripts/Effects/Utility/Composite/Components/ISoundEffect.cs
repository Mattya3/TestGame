using UnityEngine;

namespace EffectsCompositeComponent
{
    public interface ISoundEffect
    {
        bool isEnabled { get; }
        void Initialize(AudioSource audioSource);
        void Play();
    }
}

