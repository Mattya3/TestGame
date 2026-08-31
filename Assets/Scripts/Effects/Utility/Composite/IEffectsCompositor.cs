using UnityEngine;

public interface IEffectsCompositor
{
    public void Initialize(
        AudioSource audioSource,
        CameraMutableAccess cameraAccess,
        TransformOffsetController transformOffsetController,
        Renderer renderer
        );

    public void PlayEffects();
    public void StopEffects();
}
