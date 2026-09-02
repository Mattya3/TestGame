using UnityEngine;

public interface IEffectsCompositor
{
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
