using UnityEngine;

public class GoalEntityBaseShaderProperties : MonoMaterialAccessBehaviour
{
    [SerializeField]
    private Vector3 _illuminationThresholds = Vector3.one;

    private readonly int ILLUMINATION_THRESHOLDS_PROPERTY_ID = Shader.PropertyToID(
        "_IlluminationThresholds"
    );

    protected override void SetMaterialProperties(MaterialPropertyBlock materialPropertyBlock)
    {
        materialPropertyBlock.SetVector(
            ILLUMINATION_THRESHOLDS_PROPERTY_ID,
            _illuminationThresholds
        );
    }

    protected override bool IsDirty
    {
        get { return false; }
    }
}
