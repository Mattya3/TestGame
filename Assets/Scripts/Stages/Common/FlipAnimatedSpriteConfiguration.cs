using UnityEngine;

[RequireComponent(typeof(Renderer))]
[ExecuteInEditMode]
public class FlipAnimatedSpriteConfiguration : MonoMaterialAccessBehaviour
{
    [SerializeField]
    private Vector2Int _numSlices = Vector2Int.one;

    private const string NUM_SLICES_PROPERTY_NAME = "_NumSlices";

    protected override void SetMaterialProperties(MaterialPropertyBlock materialPropertyBlock)
    {
        var floatNumSlices = new Vector2(_numSlices.x, _numSlices.y);
        materialPropertyBlock.SetVector(NUM_SLICES_PROPERTY_NAME, floatNumSlices);
    }
}
