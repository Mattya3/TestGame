using System;
using UnityEngine;

[Serializable]
public class CameraTargetShift
{
    [SerializeField]
    private Vector2 _velocityCoeff;

    [SerializeField]
    private Vector2 _maxShiftAmount;

    private Vector3 _shift = Vector3.zero;
    private Vector3 _prevTargetPos = Vector3.zero;

    public Vector3 Shift => _shift;

    public void Start(Vector3 targetPos)
    {
        _prevTargetPos = targetPos;
    }

    public void LateUpdate(Vector3 targetPos)
    {
        var deltaTargetPos = targetPos - _prevTargetPos;
        var deltaShift = Vector3.Scale(new Vector3(_velocityCoeff.x, _velocityCoeff.y, 0), deltaTargetPos);

        _shift += deltaShift;
        _shift = new Vector3(
            Mathf.Clamp(_shift.x, -_maxShiftAmount.x, _maxShiftAmount.x),
            Mathf.Clamp(_shift.y, -_maxShiftAmount.y, _maxShiftAmount.y),
            _shift.z
            );

        _prevTargetPos = targetPos;
    }
}
