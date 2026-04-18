using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MonoMaterialAccessBehaviour : MonoBehaviour
{
    protected Material _material;

    protected virtual void Awake()
    {
        _material = GetComponent<Renderer>().material;
        if (_material == null)
        {
            Debug.LogError("Renderer does not have a material.", this);
            enabled = false;
            return;
        }
    }

    protected virtual void OnDestroy()
    {
        // 明示的にマテリアルを破棄して、メモリリークを防止
        if (_material != null)
        {
            Destroy(_material);
            _material = null;
        }
    }
}
