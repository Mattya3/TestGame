using UnityEngine;

public interface IEffectsCompositor
{
    public bool IsRoot { get; }
    public bool PlayInUnscaledTime { get; }
    public float PlaybackSpeedRate { get; }

    public void Initialize(
        SoundEffectOutput soundEffectOutput,
        CameraMutableAccess cameraAccess,
        LocalTransformOffsetController transformOffsetController,
        Renderer renderer,
        Transform instantiationParent
    );

    public void PlayEffects();
    public void StopEffects();

    public void SetVFXFloatProperty(int propertyId, float value);
    public void SetVFXVector2Property(int propertyId, Vector2 value);
}
