using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class WipingMaskController : MonoBehaviour
{
    [SerializeField]
    private float _maskThreshold = 0.0f;

    private Material _material;

    private static readonly int MaskThresholdID = Shader.PropertyToID("_MaskThreshold");

    private void Awake()
    {
        _material = GetComponent<Image>().material;
        if (_material == null)
        {
            Debug.LogError("WipingMaskController requires an Image component with a material.");
            enabled = false;
        }
    }

    private void OnDestroy()
    {
        if (_material != null)
        {
            Destroy(_material);
            _material = null;
        }
    }

    private void Update()
    {
        _material.SetFloat(MaskThresholdID, _maskThreshold);
    }
}
