using UnityEngine;

public interface IEffectsCompositor
{
    public bool IsRoot { get; }
    public bool PlayInUnscaledTime { get; }
    public float PlaySpeedRate { get; }

    public void Initialize(
        AudioSource audioSource,
        CameraMutableAccess cameraAccess,
        TransformOffsetController transformOffsetController,
        Renderer renderer,
        Transform instantiationParent
        );

    public void PlayEffects();
    public void StopEffects();
}
