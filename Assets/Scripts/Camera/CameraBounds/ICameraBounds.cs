using UnityEngine;

public interface ICameraBounds
{
    Vector3 Bound(Vector3 pos, Vector3 originalPos, Camera camera);
    void DrawGizmos();
}
