using UnityEngine;

public class WipingMaskController : MonoUIImageMaterialAccessBehaviour
{
    [SerializeField]
    private float _maskThreshold = 0.0f;

    private static readonly int MaskThresholdID = Shader.PropertyToID("_MaskThreshold");

    protected override void SetMaterialProperties(Material material)
    {
        material.SetFloat(MaskThresholdID, _maskThreshold);
    }
}
