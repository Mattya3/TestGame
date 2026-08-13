using UnityEngine;

public class CameraReadonlyAccess : AccessComponent<CameraController>
{
    public float AspectRatio => Reference?.AspectRatio ?? 1f;
}
