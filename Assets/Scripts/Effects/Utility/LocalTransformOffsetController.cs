using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

[ExecuteAlways]
public class LocalTransformOffsetController : MonoBehaviour, IAnimationWindowPreview
{
    [SerializeField]
    private Transform _targetTransform;

    private Vector3 _effectPositionOffset = Vector3.zero;
    private Vector3 _effectRotationOffset = Vector3.zero;
    private Vector3 _effectScaleOffset = Vector3.one;

    // Animatorから操作できるようSerializedFieldを付与
    [SerializeField, HideInInspector]
    private Vector3 _animationPositionOffset = Vector3.zero;
    [SerializeField, HideInInspector]
    private Vector3 _animationRotationOffset = Vector3.zero;
    [SerializeField, HideInInspector]
    private Vector3 _animationScaleOffset = Vector3.one;
    
    private Vector3 _defaultLocalPosition;
    private Quaternion _defaultLocalRotation;
    private Vector3 _defaultLocalScale;
    private bool _isDirty = true;
    private bool _isPreviewing = false;

    private void Awake()
    {
        if (_targetTransform == null)
        {
            Debug.LogError("Target Transform is not assigned.", this);
            return;
        }

        _MemorizeDefaultLocalTransform();
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

    public void StartPreview()
    {
        if (_isPreviewing)
            return;

        _MemorizeDefaultLocalTransform();
        _isPreviewing = true;
    }

    public void StopPreview()
    {
        if (!_isPreviewing)
            return;

        _isPreviewing = false;
        _ResetToDefaultLocalTransform();
    }

    public Playable BuildPreviewGraph(
        PlayableGraph graph, Playable input)
    {
        return input;
    }

    public void UpdatePreviewGraph(PlayableGraph graph) {}

    private void OnDisable()
    {
        StopPreview();
    }

    private void LateUpdate()
    {
        if (!_isDirty)
            return;
            
        if (Application.IsPlaying(gameObject) || _isPreviewing)
            _ApplyOffsets();
    }

    private void OnDidApplyAnimationProperties()
    {
        _isDirty = true;

        if (Application.IsPlaying(gameObject) || _isPreviewing)
            _ApplyOffsets();
    }

    private void _MemorizeDefaultLocalTransform()
    {
        if (_targetTransform == null)
            return;

        _defaultLocalPosition = _targetTransform.localPosition;
        _defaultLocalRotation = _targetTransform.localRotation;
        _defaultLocalScale = _targetTransform.localScale;
    }

    private void _ResetToDefaultLocalTransform()
    {
        if (_targetTransform == null)
            return;

        _targetTransform.localPosition = _defaultLocalPosition;
        _targetTransform.localRotation = _defaultLocalRotation;
        _targetTransform.localScale = _defaultLocalScale;
    }

    private void _ApplyOffsets()
    {
        if (_targetTransform == null)
            return;

        _targetTransform.localPosition = _defaultLocalPosition + _effectPositionOffset + _animationPositionOffset;
        _targetTransform.localRotation = _defaultLocalRotation * Quaternion.Euler(_effectRotationOffset + _animationRotationOffset);
        _targetTransform.localScale = Vector3.Scale(_defaultLocalScale, Vector3.Scale(_effectScaleOffset, _animationScaleOffset));
        _isDirty = false;
    }
}
