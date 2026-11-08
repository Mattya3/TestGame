using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(CameraReadonlyAccess))]
public class CameraTracker : MonoBehaviour
{
    [SerializeField]
    private Vector3 _offset = Vector3.zero;

    [SerializeField]
    private bool3 _trackAxes = new bool3(false, false, false);

    private CameraReadonlyAccess _cameraAccess;

    private void Awake()
    {
        _cameraAccess = GetComponent<CameraReadonlyAccess>();
    }

    private void LateUpdate()
    {
        var newPosition = _cameraAccess.UnoffsetPosition + _offset;
        if (!_trackAxes.x) newPosition.x = transform.position.x;
        if (!_trackAxes.y) newPosition.y = transform.position.y;
        if (!_trackAxes.z) newPosition.z = transform.position.z;
        transform.position = newPosition;
    }
}
