using UnityEngine;

public interface ICameraTarget
{
    bool IsActive { get; }

    Vector3 Position { get; }

    bool AreCollidersEnabled { get; }

    void OnStart();
}
