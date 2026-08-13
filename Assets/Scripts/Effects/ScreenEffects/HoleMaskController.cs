using UnityEngine;

[RequireComponent(typeof(CameraReadonlyAccess))]
public class HoleMaskController : MonoUIImageMaterialAccessBehaviour
{
    private const float DEFAULT_ASPECT_RATIO = 1.78f;

    [SerializeField]
    private float _maskThreshold = 0.0f;

    private float _lastMaskThreshold = float.MaxValue;
    private CameraReadonlyAccess _cameraAccess;

    private static readonly int MaskThresholdID = Shader.PropertyToID("_MaskThreshold");
    private static readonly int CameraAspectID = Shader.PropertyToID("_CameraAspect");

    protected override void Awake()
    {
        base.Awake();
        _cameraAccess = GetComponent<CameraReadonlyAccess>();
    }

    protected override bool IsMaterialValid(Material material)
    {
        if (!material.HasProperty(MaskThresholdID))
        {
            Debug.LogError(
                $"Material {material.name} does not have a property named '_MaskThreshold'."
            );
            return false;
        }
        if (!material.HasProperty(CameraAspectID))
        {
            Debug.LogError(
                $"Material {material.name} does not have a property named '_CameraAspect'."
            );
            return false;
        }
        return true;
    }

    protected override void SetMaterialProperties(Material material)
    {
        material.SetFloat(MaskThresholdID, _maskThreshold);
        material.SetFloat(CameraAspectID, _cameraAccess?.AspectRatio ?? DEFAULT_ASPECT_RATIO);
        _lastMaskThreshold = _maskThreshold;
    }

    protected override bool IsDirty => !Mathf.Approximately(_maskThreshold, _lastMaskThreshold);
}
