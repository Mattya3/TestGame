using UnityEngine;

public class FixedCameraTarget : MonoBehaviour, ICameraTarget
{
    [SerializeField]
    private Vector3 _position;

    public bool IsActive => enabled;

    public void OnStart()
    {
        enabled = true;
    }

    public Vector3 Position => _position;

    public Vector3 AnimatedShift => Vector3.zero;

    public bool AreCollidersEnabled => false;
}
