using UnityEngine;

public class CameraMutableAccess : AccessComponent<CameraController>
{
    public void PushTarget(ICameraTarget target)
    {
        Reference?.PushTarget(target);
    }

    public void PopTarget()
    {
        Reference?.PopTarget();
    }
}
