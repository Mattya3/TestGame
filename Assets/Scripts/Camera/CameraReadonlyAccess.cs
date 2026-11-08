using UnityEngine;

public class CameraReadonlyAccess : AccessComponent<CameraController>
{
    public float AspectRatio
    {
        get
        {
            if (!Reference)
                return 1.0f;

            Reference.EnsureInitialized();
            return Reference.AspectRatio;
        }
    }

    public Vector3 WorldToViewportPoint(Vector3 worldPosition)
    {
        if (!Reference)
            return Vector3.zero;

        Reference.EnsureInitialized();
        return Reference.WorldToViewportPoint(worldPosition);
    }

    public Vector3 UnoffsetPosition
    {
        get
        {
            if (!Reference)
                return Vector3.zero;

            Reference.EnsureInitialized();
            return Reference.UnoffsetPosition;
        }
    }
}