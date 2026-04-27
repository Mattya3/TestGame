using UnityEngine;

public class CameraAccess : MonoBehaviour
{
    static private CameraController _reference;

    static public void Register(CameraController reference)
    {
        _reference = reference;
    }

    static public void Unregister(CameraController reference)
    {
        if (_reference == reference)
            _reference = null;
    }

    public void PushTarget(ICameraTarget target)
    {
        _reference?.PushTarget(target);
    }

    public void PopTarget()
    {
        _reference?.PopTarget();
    }
}
