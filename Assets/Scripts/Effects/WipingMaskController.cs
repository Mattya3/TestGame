using UnityEngine;

public class WipingMaskController : MonoUIImageMaterialAccessBehaviour
{
    [SerializeField]
    private float _maskThreshold = 0.0f;

    private float _lastMaskThreshold = float.MaxValue;

    private static readonly int MaskThresholdID = Shader.PropertyToID("_MaskThreshold");

    protected override void SetMaterialProperties(Material material)
    {
        material.SetFloat(MaskThresholdID, _maskThreshold);
        _lastMaskThreshold = _maskThreshold;
    }

    protected override bool IsDirty => !Mathf.Approximately(_maskThreshold, _lastMaskThreshold);
}
