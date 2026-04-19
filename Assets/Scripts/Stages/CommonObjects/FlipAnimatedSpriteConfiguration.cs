using UnityEngine;

[RequireComponent(typeof(Renderer))]
[ExecuteInEditMode]
public class FlipAnimatedSpriteConfiguration : MonoBehaviour
{
    [SerializeField]
    private Vector2Int _numSlices = Vector2Int.one;

    private const string NUM_SLICES_PROPERTY_NAME = "_NumSlices";

    private void OnValidate()
    {
        Apply();
    }

    private void Awake()
    {
        Apply();
    }

    private void Apply()
    {
        var renderer = GetComponent<Renderer>();
        var materialPropertyBlock = new MaterialPropertyBlock();

        var floatNumSlices = new Vector2(_numSlices.x, _numSlices.y);

        renderer.GetPropertyBlock(materialPropertyBlock);
        materialPropertyBlock.SetVector(NUM_SLICES_PROPERTY_NAME, floatNumSlices);
        renderer.SetPropertyBlock(materialPropertyBlock);
    }
}
