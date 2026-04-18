using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class GoalEntityBaseShaderProperties : MonoBehaviour
{
    [SerializeField]
    private Vector3 _illuminationThresholds = Vector3.one;

    private Material _material;

    private const string ILLUMINATION_THRESHOLDS_PROPERTY_NAME = "_IlluminationThresholds";

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
        if (_material == null)
        {
            Debug.LogError("Renderer does not have a material.", this);
            enabled = false;
            return;
        }
    }

    private void OnDestroy()
    {
        // 明示的にマテリアルを破棄して、メモリリークを防止
        if (_material != null)
        {
            Destroy(_material);
            _material = null;
        }
    }

    private void Update()
    {
        _material.SetVector(ILLUMINATION_THRESHOLDS_PROPERTY_NAME, _illuminationThresholds);
    }
}
