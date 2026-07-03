using UnityEngine;

public class WipingMaskController : MonoUIImageMaterialAccessBehaviour
{
    [SerializeField]
    private float _maskThreshold = 0.0f;

    private float _lastMaskThreshold = float.MaxValue;

    private static readonly int MaskThresholdID = Shader.PropertyToID("_MaskThreshold");

    protected override bool IsMaterialValid(Material material)
    {
        if (!material.HasProperty(MaskThresholdID))
        {
            Debug.LogError($"Material {material.name} does not have a property named '_MaskThreshold'.");
            return false;
        }
        return true;
    }

    protected override void SetMaterialProperties(Material material)
    {
        material.SetFloat(MaskThresholdID, _maskThreshold);
        _lastMaskThreshold = _maskThreshold;
    }

    protected override bool IsDirty => !Mathf.Approximately(_maskThreshold, _lastMaskThreshold);
}
