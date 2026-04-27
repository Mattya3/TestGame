using UnityEngine;

public class CustomTimeLooper : MonoMaterialAccessBehaviour
{
    [SerializeField, Min(1e-3f)]
    private float _loopDuration = 1f;

    private float _time = 0f;

    private const string TIME_PROPERTY_NAME = "_CustomTime";

    void Update()
    {
        _time += Time.deltaTime;
        if (_time >= _loopDuration)
            _time -= _loopDuration;

        _material.SetFloat(TIME_PROPERTY_NAME, _time / _loopDuration);
    }
}
