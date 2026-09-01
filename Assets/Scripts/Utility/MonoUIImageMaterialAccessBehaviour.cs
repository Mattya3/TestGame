using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[ExecuteInEditMode]
public class MonoUIImageMaterialAccessBehaviour : MonoBehaviour, IMaterialModifier
{
    private Image _image;
    private Material _instancedMaterial;
    private bool _hasInvalidMaterial = false;
    private Material _lastCheckedBaseMaterial = null;

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
        // 1. ベースマテリアルのnullチェック
        if (baseMaterial == null)
        {
            _MarkMaterialAsInvalid();
            return null;
        }

        // 2. ベースマテリアル変更時の処理
        if (_IsBaseMaterialChanged(baseMaterial))
        {
            _HandleBaseMaterialChange(baseMaterial);
        }

        // 3. 無効状態の場合はベースマテリアルをそのまま返す
        if (_hasInvalidMaterial)
            return baseMaterial;

        // 4. マテリアルの検証
        if (!IsMaterialValid(baseMaterial))
        {
            _MarkMaterialAsInvalid();
            return baseMaterial;
        }

        // 5. インスタンスマテリアルの取得または生成
        Material instancedMaterial = _GetOrCreateInstancedMaterial(baseMaterial);

        // 6. プロパティのコピーと設定
        _UpdateMaterialProperties(instancedMaterial, baseMaterial);

        return instancedMaterial;
    }

    private void _MarkMaterialAsInvalid()
    {
        _lastCheckedBaseMaterial = null;
        _hasInvalidMaterial = true;
    }

    private bool _IsBaseMaterialChanged(Material baseMaterial)
    {
        return _lastCheckedBaseMaterial != baseMaterial;
    }

    private void _HandleBaseMaterialChange(Material baseMaterial)
    {
        _lastCheckedBaseMaterial = baseMaterial;
        _hasInvalidMaterial = false;
        _CleanupMaterial();
    }

    private Material _GetOrCreateInstancedMaterial(Material baseMaterial)
    {
        // インスタンスが存在しない場合は新規作成
        if (_instancedMaterial == null)
        {
            return _CreateInstancedMaterial(baseMaterial);
        }

        // シェーダーが変更された場合は再生成
        if (_instancedMaterial.shader != baseMaterial.shader)
        {
            _CleanupMaterial();
            return _CreateInstancedMaterial(baseMaterial);
        }

        return _instancedMaterial;
    }

    private Material _CreateInstancedMaterial(Material baseMaterial)
    {
        _instancedMaterial = new Material(baseMaterial);
        _instancedMaterial.hideFlags = HideFlags.HideAndDontSave;
        return _instancedMaterial;
    }

    private void _UpdateMaterialProperties(Material instancedMaterial, Material baseMaterial)
    {
        instancedMaterial.CopyPropertiesFromMaterial(baseMaterial);
        SetMaterialProperties(instancedMaterial);
    }
}
