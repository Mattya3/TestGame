using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CameraTargetShift
{
    [SerializeField]
    private Vector2 _velocityCoeff;

    [SerializeField]
    private Vector2 _maxShiftAmount;

    [SerializeField]
    private Vector2 _distanceDampThreshold;

    [SerializeField]
    private Vector2 _distanceDampAmount;

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
        if (_distanceDampThreshold.x < 0 || _distanceDampThreshold.y < 0)
        {
            Debug.LogError("distanceDampThresholdの値は負の値であってはなりません");
            _distanceDampThreshold = Vector2.Max(_distanceDampThreshold, Vector2.zero);
        }
        if (_distanceDampAmount.x < 0 || _distanceDampAmount.y < 0)
        {
            Debug.LogError("distanceDampAmountの値は負の値であってはなりません");
            _distanceDampAmount = Vector2.Max(_distanceDampAmount, Vector2.zero);
        }
    }

    public void Start(Vector3 targetPos)
    {
        _prevTargetPos = targetPos;
    }

    public void LateUpdate(Vector3 targetPos, Vector2 distanceDampFactor)
    {
        var delta = targetPos - _prevTargetPos;
        var scaledDelta = Vector3.Scale(new Vector3(_velocityCoeff.x, _velocityCoeff.y, 0), delta);

        var dampedMaxShiftAmount = _CalculateDampedMaxShiftAmount(distanceDampFactor);

        _shift += scaledDelta;
        _shift = new Vector3(
            Mathf.Clamp(_shift.x, -dampedMaxShiftAmount.x, dampedMaxShiftAmount.x),
            Mathf.Clamp(_shift.y, -dampedMaxShiftAmount.y, dampedMaxShiftAmount.y),
            _shift.z
        );

        _prevTargetPos = targetPos;
    }

    private Vector2 _CalculateDampedMaxShiftAmount(Vector2 distanceDampFactor)
    {
        var dampExceeds = Vector2.Max(Vector2.zero, distanceDampFactor - _distanceDampThreshold);
        var dampScaledExceeds = Vector2.Scale(dampExceeds, _distanceDampAmount);
        var dampSquaredExceeds = _CalculateExceedsCurve(dampScaledExceeds); 
        var dampScaler = Vector2.one - Vector2.Min(Vector2.one, dampSquaredExceeds);
        return Vector2.Scale(dampScaler, _maxShiftAmount);
    }

    private Vector2 _CalculateExceedsCurve(Vector2 scaledExceeds)
    {
        // パターン1: 線形減衰
        // return scaledExceeds;

        // パターン2:  二次関数的に減衰させる。0付近で勾配が連続になる。
        //return Vector2.Scale(scaledExceeds, scaledExceeds);

        // パターン3: 三次関数。0と1付近で勾配が連続になる。
        var cubed = Vector2.Scale(scaledExceeds, Vector2.Scale(scaledExceeds, scaledExceeds));
        var squared = Vector2.Scale(scaledExceeds, scaledExceeds);
        return -0.5f * cubed + 1.5f * squared;
    }
}
