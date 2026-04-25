using UnityEngine;

public interface ICameraTarget
{
    Vector3 Position { get; }

    void OnStart();
}
