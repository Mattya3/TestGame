using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class CustomTimeLooper : MonoMaterialAccessBehaviour
{
    [SerializeField, Min(0.0f)]
    private float _loopDuration = 1f;

    private float _time = 0f;

    private const string TIME_PROPERTY_NAME = "_CustomTime";

    private void Update()
    {
        _time += Time.deltaTime;
        if (_time >= _loopDuration)
            _time -= _loopDuration;
    }

    protected override void SetMaterialProperties(MaterialPropertyBlock materialPropertyBlock)
    {
        materialPropertyBlock.SetFloat(TIME_PROPERTY_NAME, _time / _loopDuration);
    }
}
