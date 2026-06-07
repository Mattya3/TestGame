using UnityEngine;

public class FlipAnimatedSpriteConfiguration : MonoMaterialAccessBehaviour
{
    [SerializeField]
    private Vector2Int _numSlices = Vector2Int.one;

    [SerializeField, Range(0, 16)]
    private int _patternIndexOffset = 0;

    private readonly int NUM_SLICES_PROPERTY_ID = Shader.PropertyToID("_NumSlices");
    private readonly int PATTERN_INDEX_OFFSET_PROPERTY_ID = Shader.PropertyToID(
        "_PatternIndexOffset"
    );

    protected override void OnValidate()
    {
        if (_patternIndexOffset < 0 || _patternIndexOffset >= _numSlices.x)
            _patternIndexOffset = Mathf.Clamp(_patternIndexOffset, 0, _numSlices.x - 1);

        base.OnValidate();
    }

    protected override void SetMaterialProperties(MaterialPropertyBlock materialPropertyBlock)
    {
        var floatNumSlices = new Vector2(_numSlices.x, _numSlices.y);
        materialPropertyBlock.SetVector(NUM_SLICES_PROPERTY_ID, floatNumSlices);
        materialPropertyBlock.SetFloat(PATTERN_INDEX_OFFSET_PROPERTY_ID, _patternIndexOffset);
    }

    protected override bool IsDirty
    {
        get { return false; }
    }
}
