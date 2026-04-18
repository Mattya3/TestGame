using UnityEngine;

public class GoalEntityBaseMaterialConfiguration : SciFiBlockMaterialConfiguration
{
    [SerializeField]
    private Vector3 _illuminationThresholds = Vector3.one;

    private const string ILLUMINATION_THRESHOLDS_PROPERTY_NAME = "_IlluminationThresholds";

    private void Update()
    {
        Apply();
    }

    protected override void _SetAdditionalProperties(MaterialPropertyBlock materialPropertyBlock)
    {
        base._SetAdditionalProperties(materialPropertyBlock);
        materialPropertyBlock.SetVector(ILLUMINATION_THRESHOLDS_PROPERTY_NAME, _illuminationThresholds);
    }
}
