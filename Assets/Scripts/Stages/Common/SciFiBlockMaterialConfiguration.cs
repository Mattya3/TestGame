using UnityEngine;

public class SciFiBlockMaterialConfiguration : MonoMaterialAccessBehaviour
{
    [SerializeField]
    private SciFiBlockMaskColor _maskColor = null;

    [SerializeField]
    private SciFiBlockIllumination _illumination = null;

    private readonly int MASK_COLOR_PROPERTY_ID = Shader.PropertyToID("_MaskColor");
    private readonly int ILLUMINATION_COLOR1_PROPERTY_ID = Shader.PropertyToID(
        "_IlluminationColor1"
    );
    private readonly int ILLUMINATION_COLOR2_PROPERTY_ID = Shader.PropertyToID(
        "_IlluminationColor2"
    );
    private readonly int ILLUMINATION_COLOR3_PROPERTY_ID = Shader.PropertyToID(
        "_IlluminationColor3"
    );
    private readonly int ILLUMINATION_INTENSITY_PROPERTY_ID = Shader.PropertyToID(
        "_IlluminationIntensity"
    );
    private readonly int ILLUMINATION_OFFSET_VECTOR_PROPERTY_ID = Shader.PropertyToID(
        "_IlluminationOffsetVector"
    );

    protected override void SetMaterialProperties(MaterialPropertyBlock materialPropertyBlock)
    {
        materialPropertyBlock.SetColor(MASK_COLOR_PROPERTY_ID, _maskColor.color);
        materialPropertyBlock.SetColor(ILLUMINATION_COLOR1_PROPERTY_ID, _illumination.color1);
        materialPropertyBlock.SetColor(ILLUMINATION_COLOR2_PROPERTY_ID, _illumination.color2);
        materialPropertyBlock.SetColor(ILLUMINATION_COLOR3_PROPERTY_ID, _illumination.color3);
        materialPropertyBlock.SetFloat(ILLUMINATION_INTENSITY_PROPERTY_ID, _illumination.intensity);
        materialPropertyBlock.SetVector(
            ILLUMINATION_OFFSET_VECTOR_PROPERTY_ID,
            _illumination.offsetVector
        );
    }

    protected override bool IsDirty
    {
        get { return false; }
    }
}
