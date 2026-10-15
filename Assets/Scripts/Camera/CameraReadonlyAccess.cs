using UnityEngine;

public class CameraReadonlyAccess : AccessComponent<CameraController>
{
    public float AspectRatio => Reference?.AspectRatio ?? 1f;

    public Vector3 WorldToViewportPoint(Vector3 worldPosition)
    {
        return Reference != null ? Reference.WorldToViewportPoint(worldPosition) : Vector3.zero;
    }
}
