using UnityEngine;

public class TransformOffsetController : MonoBehaviour
{
    [SerializeField]
    private Transform _originTransform;

    [SerializeField]
    private Transform _targetTransform;

    private Vector3 _effectPositionOffset = Vector3.zero;
    private Vector3 _effectRotationOffset = Vector3.zero;
    private Vector3 _effectScaleOffset = Vector3.one;

    // Animatorから操作できるようSerializedFieldを付与
    [SerializeField]
    private Vector3 _animationPositionOffset = Vector3.zero;
    [SerializeField]
    private Vector3 _animationRotationOffset = Vector3.zero;
    [SerializeField]
    private Vector3 _animationScaleOffset = Vector3.one;
    
    private bool _isDirty = false;

    private void Awake()
    {
        if (_originTransform == null)
        {
            Debug.LogError("Origin Transform is not assigned.", this);
            return;
        }
        if (_targetTransform == null)
        {
            Debug.LogError("Target Transform is not assigned.", this);
            return;
        }
    }

    public void SetEffectPositionOffset(Vector3 positionOffset)
    {
        _effectPositionOffset = positionOffset;
        _isDirty = true;
    }

    public void SetEffectRotationOffset(Vector3 rotationOffset)
    {
        _effectRotationOffset = rotationOffset;
        _isDirty = true;
    }

    public void SetEffectScaleOffset(Vector3 scaleOffset)
    {
        _effectScaleOffset = scaleOffset;
        _isDirty = true;
    }

    private void LateUpdate()
    {
        if (!_isDirty)
            return;

        if (_originTransform == null || _targetTransform == null)
            return;

        _targetTransform.position = _originTransform.position + _effectPositionOffset + _animationPositionOffset;
        _targetTransform.rotation = _originTransform.rotation * Quaternion.Euler(_effectRotationOffset + _animationRotationOffset);
        _targetTransform.localScale = Vector3.Scale(_originTransform.localScale, Vector3.Scale(_effectScaleOffset, _animationScaleOffset));
        _isDirty = false;
    }

    private void OnDidApplyAnimationProperties()
    {
        _isDirty = true;
    }
}
