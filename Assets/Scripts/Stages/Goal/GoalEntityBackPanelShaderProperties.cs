using UnityEngine;

public class GoalEntityBackPanelShaderProperties : MonoMaterialAccessBehaviour
{
    [SerializeField]
    private float _illuminationThreshold = 1.0f;

    private const string ILLUMINATION_THRESHOLD_PROPERTY_NAME = "_IlluminationThreshold";

    private void Update()
    {
        _material.SetFloat(ILLUMINATION_THRESHOLD_PROPERTY_NAME, _illuminationThreshold);
    }
}
