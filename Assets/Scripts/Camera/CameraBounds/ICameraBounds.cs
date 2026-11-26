using UnityEngine;

public interface ICameraBounds : IAccessTarget
{
    Vector3 Bound(Vector3 pos, Vector3 originalPos, Camera camera);
    void DrawGizmos();
}
