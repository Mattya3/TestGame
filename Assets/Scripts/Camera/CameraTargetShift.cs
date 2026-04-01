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

    private const float VELOCITY_SCALE_FACTOR = 0.9f;
    private const float EPSILON = 1e-3f;

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

    public void LateUpdate(Vector3 targetPos, Vector2 damp)
    {
        var dampedMaxShiftAmount = Vector2.Scale(_maxShiftAmount, damp);

        var delta = targetPos - _prevTargetPos;
        var velocityScale = _CalculateVelocityScale(dampedMaxShiftAmount);
        var scaledDelta = Vector3.Scale(new Vector3(_velocityCoeff.x * velocityScale.x, _velocityCoeff.y * velocityScale.y, 0), delta);

        _shift += scaledDelta;
        _shift = new Vector3(
            Mathf.Clamp(_shift.x, -dampedMaxShiftAmount.x, dampedMaxShiftAmount.x),
            Mathf.Clamp(_shift.y, -dampedMaxShiftAmount.y, dampedMaxShiftAmount.y),
            _shift.z
        );

        _prevTargetPos = targetPos;
    }

    public Vector3 Get()
    {
        return _shift;
    }

    // シフト量がクリッピング境界に近づくほど、速度を減衰させるスケールを計算
    // これにより、シフトが最大値に達する前に速度が減少し、滑らかな動きになる
    // 進行方向から一瞬だけ切り返したときに、シフトが急激に反転するのを防止する効果もある
    private Vector2 _CalculateVelocityScale(Vector2 dampedShiftAmount)
    {
        var normalizedShift = new Vector2(
            Mathf.Abs(_shift.x) / Mathf.Max(dampedShiftAmount.x, EPSILON),
            Mathf.Abs(_shift.y) / Mathf.Max(dampedShiftAmount.y, EPSILON)
        );
        return Vector2.one - Vector2.Min(Vector2.one, normalizedShift) * VELOCITY_SCALE_FACTOR;
    }
}
