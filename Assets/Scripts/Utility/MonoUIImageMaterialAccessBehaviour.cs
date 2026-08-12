using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[ExecuteInEditMode]
public class MonoUIImageMaterialAccessBehaviour : MonoBehaviour, IMaterialModifier
{
    private Image _image;
    private Material _instancedMaterial;
    private bool _hasInvalidMaterial = false;

    protected virtual void OnValidate()
    {
        _GetReferences();
        _Apply();
    }

    protected virtual void Awake()
    {
        _GetReferences();
    }

    protected virtual void OnEnable()
    {
        _Apply();
    }

    protected virtual void LateUpdate()
    {
        if (_hasInvalidMaterial)
            return;

        if (IsDirty)
            _Apply();
    }

    protected virtual void OnDisable()
    {
        _CleanupMaterial();
    }

    protected virtual void OnDestroy()
    {
        _CleanupMaterial();
    }

    protected virtual bool IsMaterialValid(Material material)
    {
        // Override this method in derived classes to validate the material.
        return true;
    }

    protected virtual void SetMaterialProperties(Material material)
    {
        // Override this method in derived classes to set properties on the material.
    }

    protected virtual bool IsDirty
    {
        get { return true; }
    }

    private void _GetReferences()
    {
        if (_image == null)
            _image = GetComponent<Image>();
    }

    private void _Apply()
    {
        if (_image == null)
            return;

        if (_hasInvalidMaterial)
            return;

        _image.SetMaterialDirty();
    }

    private void _CleanupMaterial()
    {
        if (_instancedMaterial == null)
            return;

        if (Application.isPlaying)
            Destroy(_instancedMaterial);
        else
            DestroyImmediate(_instancedMaterial);

        _instancedMaterial = null;
    }

    public Material GetModifiedMaterial(Material baseMaterial)
    {
        if (baseMaterial == null)
            return null;

        if (_hasInvalidMaterial)
            return baseMaterial;

        if (!IsMaterialValid(baseMaterial))
        {
            _hasInvalidMaterial = true;
            return baseMaterial;
        }

        if (_instancedMaterial == null)
        {
            _instancedMaterial = new Material(baseMaterial);
            _instancedMaterial.hideFlags = HideFlags.HideAndDontSave;
        }
        else if (_instancedMaterial.shader != baseMaterial.shader)
        {
            // ベースマテリアルのシェーダーが変更された場合は再生成
            _CleanupMaterial();
            _instancedMaterial = new Material(baseMaterial);
            _instancedMaterial.hideFlags = HideFlags.HideAndDontSave;
        }
        _instancedMaterial.CopyPropertiesFromMaterial(baseMaterial);

        SetMaterialProperties(_instancedMaterial);

        return _instancedMaterial;
    }
}
