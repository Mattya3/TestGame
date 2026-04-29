using UnityEngine;

public class FlipAnimatedSpriteConfiguration : MonoMaterialAccessBehaviour
{
    [SerializeField]
    private Vector2Int _numSlices = Vector2Int.one;

    [SerializeField, Range(0, 16)]
    private int _patternIndexOffset = 0;

    private const string NUM_SLICES_PROPERTY_NAME = "_NumSlices";
    private const string PATTERN_INDEX_OFFSET_PROPERTY_NAME = "_PatternIndexOffset";

    protected override void OnValidate()
    {
        if (_patternIndexOffset < 0 || _patternIndexOffset >= _numSlices.x)
            _patternIndexOffset = Mathf.Clamp(_patternIndexOffset, 0, _numSlices.x - 1);

        base.OnValidate();
    }

    protected override void SetMaterialProperties(MaterialPropertyBlock materialPropertyBlock)
    {
        var floatNumSlices = new Vector2(_numSlices.x, _numSlices.y);
        materialPropertyBlock.SetVector(NUM_SLICES_PROPERTY_NAME, floatNumSlices);
        materialPropertyBlock.SetFloat(PATTERN_INDEX_OFFSET_PROPERTY_NAME, _patternIndexOffset);
    }
}
