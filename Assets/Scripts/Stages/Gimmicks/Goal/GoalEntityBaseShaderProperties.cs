using UnityEngine;

public class GoalEntityBaseShaderProperties : MonoMaterialAccessBehaviour
{
    [SerializeField]
    private Vector3 _illuminationThresholds = Vector3.one;

    private const string ILLUMINATION_THRESHOLDS_PROPERTY_NAME = "_IlluminationThresholds";

    protected override void SetMaterialProperties(MaterialPropertyBlock materialPropertyBlock)
    {
        materialPropertyBlock.SetVector(
            ILLUMINATION_THRESHOLDS_PROPERTY_NAME,
            _illuminationThresholds
        );
    }

    protected override bool IsDirty
    {
        get { return false; }
    }
}
