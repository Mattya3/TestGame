using UnityEngine;

public class SortingLayerController : MonoBehaviour
{
    private static readonly string ISOLATED_LIGHTING_LAYER_NAME = "IsolatedLighting";

    [SerializeField]
    private Renderer _renderer;

    private int _originalSortingLayerID = 0;
    private int _isolatedLightingLayerID = 0;

    private void Awake()
    {
        if (_renderer == null)
        {
            _renderer = GetComponentInChildren<Renderer>();
            if (_renderer == null)
            {
                Debug.LogError("Renderer component not found on SortingLayerController GameObject.");
                enabled = false;
                return;
            }
        }
        _originalSortingLayerID = _renderer.sortingLayerID;
        _isolatedLightingLayerID = SortingLayer.NameToID(ISOLATED_LIGHTING_LAYER_NAME);
    }

    public void SetIsolatedLightingLayer()
    {
        if (_renderer == null)
            return;
        _renderer.sortingLayerID = _isolatedLightingLayerID;
    }

    public void ResetSortingLayer()
    {
        if (_renderer == null)
            return;
        _renderer.sortingLayerID = _originalSortingLayerID;
    }
}
