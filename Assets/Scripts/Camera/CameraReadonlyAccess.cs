using UnityEngine;

public class CameraReadonlyAccess : AccessComponent<CameraController>
{
    public float AspectRatio => Reference?.AspectRatio ?? 1.0f;

    public Vector3 WorldToViewportPoint(Vector3 worldPosition)
    {
        return Reference?.WorldToViewportPoint(worldPosition) ?? Vector3.zero;
    }

    public Vector3 UnoffsetPosition => Reference?.UnoffsetPosition ?? Vector3.zero;
}