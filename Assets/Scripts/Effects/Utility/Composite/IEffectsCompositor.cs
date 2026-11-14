using UnityEngine;

public interface IEffectsCompositor
{
    public bool IsRoot { get; }
    public bool PlayInUnscaledTime { get; }
    public float PlaybackSpeedRate { get; }

    public void Initialize(
        SoundEffectOutput soundEffectOutput,
        CameraMutableAccess cameraAccess,
        TransformOffsetController transformOffsetController,
        Renderer renderer,
        Transform instantiationParent
    );

    public void PlayEffects();
    public void StopEffects();
}
