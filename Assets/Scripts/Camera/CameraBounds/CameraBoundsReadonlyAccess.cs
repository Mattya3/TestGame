using UnityEngine;

public class CameraBoundsReadonlyAccess : AccessComponent<ICameraBounds>
{
    public Vector3 Bound(Vector3 pos, Vector3 originalPos, Camera camera)
    {
        if (HasReference)
            return Reference.Bound(pos, originalPos, camera);
        return pos;
    }
}
