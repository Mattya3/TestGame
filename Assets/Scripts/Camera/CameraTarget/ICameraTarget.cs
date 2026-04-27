using UnityEngine;

public interface ICameraTarget
{
    bool IsActive { get; }

    Vector3 Position { get; }

    bool EnableCollider { get; }

    void OnStart();
}
