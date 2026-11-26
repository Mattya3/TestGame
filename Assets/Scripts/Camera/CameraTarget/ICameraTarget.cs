using UnityEngine;

public interface ICameraTarget
{
    bool IsActive { get; }
    Vector3 Position { get; }
    Vector3 AnimatedShift { get; }
    bool AreCollidersEnabled { get; }

    void OnStart();
}
