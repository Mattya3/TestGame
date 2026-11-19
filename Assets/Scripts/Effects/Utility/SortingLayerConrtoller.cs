using UnityEngine;

[RequireComponent(typeof(Renderer))]

public class SortingLayerConrtoller : MonoBehaviour
{
    private static readonly int ISOLATED_LIGHTING_LAYER_ID = SortingLayer.NameToID("IsolatedLighting");

    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void SetIsolatedLightingLayer()
    {
        if (_renderer == null)
            return;
        _renderer.sortingLayerID = ISOLATED_LIGHTING_LAYER_ID;
    }
}
