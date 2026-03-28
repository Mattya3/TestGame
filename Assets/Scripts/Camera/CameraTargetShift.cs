using System;
using Unity.VisualScripting;
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

    public void Awake()
    {
        if (_velocityCoeff.x < 0 || _velocityCoeff.y < 0)
        {
            Debug.LogError("velocityCoeffの値は負の値であってはなりません");
            _velocityCoeff = Vector2.Max(_velocityCoeff, Vector2.zero);
        }
        if (_maxShiftAmount.x < 0 || _maxShiftAmount.y < 0)
        {
            Debug.LogError("maxShiftAmountの値は負の値であってはなりません");
            _maxShiftAmount = Vector2.Max(_maxShiftAmount, Vector2.zero);
        }
    }

    public void Start(Vector3 targetPos)
    {
        _prevTargetPos = targetPos;
    }

    public void LateUpdate(Vector3 targetPos)
    {
        var delta = targetPos - _prevTargetPos;
        var scaledDelta = Vector3.Scale(new Vector3(_velocityCoeff.x, _velocityCoeff.y, 0), delta);

        _shift += scaledDelta;
        _shift = new Vector3(
            Mathf.Clamp(_shift.x, -_maxShiftAmount.x, _maxShiftAmount.x),
            Mathf.Clamp(_shift.y, -_maxShiftAmount.y, _maxShiftAmount.y),
            _shift.z
        );

        _prevTargetPos = targetPos;
    }
}
