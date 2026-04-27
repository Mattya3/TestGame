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

    static CameraAccess _instance;

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
        _reference?.PushTarget(target);
    }

    public void PopTarget()
    {
        _reference?.PopTarget();
    }
}
