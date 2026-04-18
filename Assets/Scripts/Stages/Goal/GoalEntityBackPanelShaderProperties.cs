using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class GoalEntityBackPanelShaderProperties : MonoBehaviour
{
    [SerializeField]
    private float _illuminationThreshold = 1.0f;

    private Material _material;

    private const string ILLUMINATION_THRESHOLD_PROPERTY_NAME = "_IlluminationThreshold";

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
        _material.SetFloat(ILLUMINATION_THRESHOLD_PROPERTY_NAME, _illuminationThreshold);
    }
}
