using UnityEngine;

public class SciFiBlockMaterialConfiguration : MonoMaterialAccessBehaviour
{
    [SerializeField]
    private SciFiBlockMaskColor _maskColor = null;

    [SerializeField]
    private SciFiBlockIllumination _illumination = null;

    private const string MASK_COLOR_PROPERTY_NAME = "_MaskColor";
    private const string ILLUMINATION_COLOR1_PROPERTY_NAME = "_IlluminationColor1";
    private const string ILLUMINATION_COLOR2_PROPERTY_NAME = "_IlluminationColor2";
    private const string ILLUMINATION_COLOR3_PROPERTY_NAME = "_IlluminationColor3";
    private const string ILLUMINATION_INTENSITY_PROPERTY_NAME = "_IlluminationIntensity";
    private const string ILLUMINATION_OFFSET_VECTOR_PROPERTY_NAME = "_IlluminationOffsetVector";

    protected override void SetMaterialProperties(MaterialPropertyBlock materialPropertyBlock)
    {
        materialPropertyBlock.SetColor(MASK_COLOR_PROPERTY_NAME, _maskColor.color);
        materialPropertyBlock.SetColor(ILLUMINATION_COLOR1_PROPERTY_NAME, _illumination.color1);
        materialPropertyBlock.SetColor(ILLUMINATION_COLOR2_PROPERTY_NAME, _illumination.color2);
        materialPropertyBlock.SetColor(ILLUMINATION_COLOR3_PROPERTY_NAME, _illumination.color3);
        materialPropertyBlock.SetFloat(
            ILLUMINATION_INTENSITY_PROPERTY_NAME,
            _illumination.intensity
        );
        materialPropertyBlock.SetVector(
            ILLUMINATION_OFFSET_VECTOR_PROPERTY_NAME,
            _illumination.offsetVector
        );
    }

    protected override bool IsDirty { get { return false; } }
}
