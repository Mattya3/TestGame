using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class LightIntensityModulator : MonoBehaviour
{
    [SerializeField, Min(0.0f)]
    private float _frequency = 1f;

    [SerializeField, Range(0.0f, 1.0f)]
    private float _amplitude = 1f;

    private Light2D _light;
    private float _initialIntensity = 1f;
    private float _time = 0f;

    private void Awake()
    {
        _light = GetComponent<Light2D>();
        _initialIntensity = _light.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        var noise = Mathf.PerlinNoise1D(_time * _frequency);
        _light.intensity = _initialIntensity * (1f + (noise * 2f - 1f) * _amplitude);
    }
}
