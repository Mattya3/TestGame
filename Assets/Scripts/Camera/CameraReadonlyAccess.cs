using UnityEngine;

public class CameraReadonlyAccess : AccessComponent<CameraController>
{
    public float AspectRatio => Reference?.AspectRatio ?? 1f;

    public Vector3 WorldToScreenPoint(Vector3 worldPosition)
    {
        return Reference != null ? Reference.WorldToScreenPoint(worldPosition) : Vector3.zero;
    }
}
