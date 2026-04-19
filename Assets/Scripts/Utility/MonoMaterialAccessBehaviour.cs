using UnityEngine;

[RequireComponent(typeof(Renderer))]
[ExecuteInEditMode]
public class MonoMaterialAccessBehaviour : MonoBehaviour
{
    private Renderer _renderer;
    private MaterialPropertyBlock _materialPropertyBlock;

    protected virtual void OnValidate()
    {
        _GetReferences();
        _Apply();
    }

    protected virtual void Awake()
    {
        _GetReferences();
    }

    protected virtual void LateUpdate()
    {
        _Apply();
    }

    protected virtual void SetMaterialProperties(MaterialPropertyBlock materialPropertyBlock)
    {
        // Override this method in derived classes to set properties on the material property block.
    }

    private void _GetReferences()
    {
        if (_renderer == null)
            _renderer = GetComponent<Renderer>();
        if (_materialPropertyBlock == null)
            _materialPropertyBlock = new MaterialPropertyBlock();
    }

    private void _Apply()
    {
        _renderer.GetPropertyBlock(_materialPropertyBlock);
        SetMaterialProperties(_materialPropertyBlock);
        _renderer.SetPropertyBlock(_materialPropertyBlock);
    }
}
