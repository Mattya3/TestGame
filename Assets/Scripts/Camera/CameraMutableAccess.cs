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

    public void PlayShake(ShakeEffect shakeEffect, bool playInUnscaledTime, float playSpeedRate)
    {
        Reference?.PlayShake(shakeEffect, playInUnscaledTime, playSpeedRate);
    }
}
