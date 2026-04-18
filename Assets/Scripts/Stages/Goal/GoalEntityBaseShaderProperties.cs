using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class GoalEntityBaseShaderProperties : MonoMaterialAccessBehaviour
{
    [SerializeField]
    private Vector3 _illuminationThresholds = Vector3.one;

    private const string ILLUMINATION_THRESHOLDS_PROPERTY_NAME = "_IlluminationThresholds";

    private void Update()
    {
        _material.SetVector(ILLUMINATION_THRESHOLDS_PROPERTY_NAME, _illuminationThresholds);
    }
}
