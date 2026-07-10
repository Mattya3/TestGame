using UnityEngine;

public class CameraMutableAccess : AccessComponent<CameraController>
{
    static CameraMutableAccess _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("Multiple CameraAccess instances detected. This is not allowed.");
            return;
        }
        _instance = this;
    }

    private void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }

    public void PushTarget(ICameraTarget target)
    {
        Reference?.PushTarget(target);
    }

    public void PopTarget()
    {
        Reference?.PopTarget();
    }
}
