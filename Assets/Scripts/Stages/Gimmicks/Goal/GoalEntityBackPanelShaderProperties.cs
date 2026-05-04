using UnityEngine;

public class GoalEntityBackPanelShaderProperties : MonoMaterialAccessBehaviour
{
    [SerializeField]
    private float _illuminationThreshold = 1.0f;

    private readonly int ILLUMINATION_THRESHOLD_PROPERTY_ID = Shader.PropertyToID(
        "_IlluminationThreshold"
    );

    protected override void SetMaterialProperties(MaterialPropertyBlock materialPropertyBlock)
    {
        materialPropertyBlock.SetFloat(ILLUMINATION_THRESHOLD_PROPERTY_ID, _illuminationThreshold);
    }

    protected override bool IsDirty
    {
        get { return false; }
    }
}
