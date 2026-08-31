using UnityEngine;

public class TransformOffsetController : MonoBehaviour
{
    [SerializeField]
    private Transform _originTransform;

    [SerializeField]
    private Transform _targetTransform;

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

    public void SetPositionOffset(Vector3 positionOffset)
    {
        if (_originTransform == null || _targetTransform == null)
            return;

        _targetTransform.position = _originTransform.position + positionOffset;
    }

    public void SetRotationOffset(Vector3 rotationOffset)
    {
        if (_originTransform == null || _targetTransform == null)
            return;

        _targetTransform.rotation = _originTransform.rotation * Quaternion.Euler(rotationOffset);
    }

    public void SetScaleOffset(Vector3 scaleOffset)
    {
        if (_originTransform == null || _targetTransform == null)
            return;

        _targetTransform.localScale = Vector3.Scale(_originTransform.localScale, scaleOffset);
    }

    public void SetOffset(Vector3 positionOffset = default, Vector3 rotationOffset = default, Vector3 scaleOffset = default)
    {
        SetPositionOffset(positionOffset);
        SetRotationOffset(rotationOffset);
        SetScaleOffset(scaleOffset);
    }
}
