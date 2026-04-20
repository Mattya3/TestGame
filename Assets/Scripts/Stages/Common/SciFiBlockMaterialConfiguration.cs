using UnityEngine;

[RequireComponent(typeof(Renderer))]
[ExecuteInEditMode]
public class SciFiBlockMaterialConfiguration : MonoMaterialAccessBehaviour
{
    [SerializeField]
    private Color _maskColor = Color.white;

    [SerializeField]
    private Color _illuminationColor1 = Color.white;

    [SerializeField]
    private Color _illuminationColor2 = Color.white;

    [SerializeField]
    private Color _illuminationColor3 = Color.white;

    [SerializeField]
    private float _illuminationIntensity = 1f;

    [SerializeField]
    private Vector3 _illuminationOffsetVector = Vector3.right;

    private const string MASK_COLOR_PROPERTY_NAME = "_MaskColor";
    private const string ILLUMINATION_COLOR1_PROPERTY_NAME = "_IlluminationColor1";
    private const string ILLUMINATION_COLOR2_PROPERTY_NAME = "_IlluminationColor2";
    private const string ILLUMINATION_COLOR3_PROPERTY_NAME = "_IlluminationColor3";
    private const string ILLUMINATION_INTENSITY_PROPERTY_NAME = "_IlluminationIntensity";
    private const string ILLUMINATION_OFFSET_VECTOR_PROPERTY_NAME = "_IlluminationOffsetVector";

    protected override void SetMaterialProperties(MaterialPropertyBlock materialPropertyBlock)
    {
        materialPropertyBlock.SetColor(MASK_COLOR_PROPERTY_NAME, _maskColor);
        materialPropertyBlock.SetColor(ILLUMINATION_COLOR1_PROPERTY_NAME, _illuminationColor1);
        materialPropertyBlock.SetColor(ILLUMINATION_COLOR2_PROPERTY_NAME, _illuminationColor2);
        materialPropertyBlock.SetColor(ILLUMINATION_COLOR3_PROPERTY_NAME, _illuminationColor3);
        materialPropertyBlock.SetFloat(ILLUMINATION_INTENSITY_PROPERTY_NAME, _illuminationIntensity);
        materialPropertyBlock.SetVector(ILLUMINATION_OFFSET_VECTOR_PROPERTY_NAME, _illuminationOffsetVector);
    }
}
