using UnityEngine;

namespace EffectsCompositeComponent
{
    public interface ISoundEffect
    {
        void Initialize(AudioSource audioSource);
        void PlaySound();
    }
}

