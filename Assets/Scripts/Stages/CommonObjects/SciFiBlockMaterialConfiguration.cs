using UnityEngine;

[RequireComponent(typeof(Renderer))]
[ExecuteInEditMode]
public class SciFiBlockMaterialConfiguration : MonoBehaviour
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
    private Vector3 _illuminationOffsetVector = Vector3.right;

    private const string MASK_COLOR_PROPERTY_NAME = "_MaskColor";
    private const string ILLUMINATION_COLOR1_PROPERTY_NAME = "_IlluminationColor1";
    private const string ILLUMINATION_COLOR2_PROPERTY_NAME = "_IlluminationColor2";
    private const string ILLUMINATION_COLOR3_PROPERTY_NAME = "_IlluminationColor3";
    private const string ILLUMINATION_OFFSET_VECTOR_PROPERTY_NAME = "_IlluminationOffsetVector";
    private Renderer _renderer;
    private MaterialPropertyBlock _materialPropertyBlock;

    private void OnValidate()
    {
        _Initialize();
    }

    private void Awake()
    {
        _Initialize();
    }

    private void Update()
    {
        _UpdateMaterialProperties();
    }

    private void _Initialize()
    {
        _renderer = GetComponent<Renderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();
        _UpdateMaterialProperties();
    }

    private void _UpdateMaterialProperties()
    {
        _renderer.GetPropertyBlock(_materialPropertyBlock);
        _materialPropertyBlock.SetColor(MASK_COLOR_PROPERTY_NAME, _maskColor);
        _materialPropertyBlock.SetColor(ILLUMINATION_COLOR1_PROPERTY_NAME, _illuminationColor1);
        _materialPropertyBlock.SetColor(ILLUMINATION_COLOR2_PROPERTY_NAME, _illuminationColor2);
        _materialPropertyBlock.SetColor(ILLUMINATION_COLOR3_PROPERTY_NAME, _illuminationColor3);
        _materialPropertyBlock.SetVector(ILLUMINATION_OFFSET_VECTOR_PROPERTY_NAME, _illuminationOffsetVector);
        _renderer.SetPropertyBlock(_materialPropertyBlock);
    }
}
