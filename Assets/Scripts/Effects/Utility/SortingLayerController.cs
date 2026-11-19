using UnityEngine;

[RequireComponent(typeof(Renderer))]

public class SortingLayerController : MonoBehaviour
{
    private static readonly string ISOLATED_LIGHTING_LAYER_NAME = "IsolatedLighting";

    private int _isolatedLightingLayerID = 0;
    private Renderer _renderer;

    private void Awake()
    {
        _isolatedLightingLayerID = SortingLayer.NameToID(ISOLATED_LIGHTING_LAYER_NAME);
        _renderer = GetComponent<Renderer>();
    }

    public void SetIsolatedLightingLayer()
    {
        if (_renderer == null)
            return;
        _renderer.sortingLayerID = _isolatedLightingLayerID;
    }
}
