using UnityEngine;

public class GoalEntityBackPanelMaterialConfiguration : SciFiBlockMaterialConfiguration
{
    [SerializeField]
    private float _illuminationThreshold = 1.0f;

    private const string ILLUMINATION_THRESHOLD_PROPERTY_NAME = "_IlluminationThreshold";

    private void Update()
    {
        Apply();
    }

    protected override void _SetAdditionalProperties(MaterialPropertyBlock materialPropertyBlock)
    {
        base._SetAdditionalProperties(materialPropertyBlock);
        materialPropertyBlock.SetFloat(ILLUMINATION_THRESHOLD_PROPERTY_NAME, _illuminationThreshold);
    }
}
