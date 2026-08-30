using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace EffectsCompositeComponent
{
    [RequireComponent(typeof(Light2D))]
    public class LightIntensityController : MonoBehaviour, ILightSourceController
    {
        [SerializeField]
        private AnimationCurve _intensityCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

        [SerializeField, Min(1e-3f)]
        private float _curveDuration = 1f;

        [SerializeField, Min(0.0f)]
        private float _modulationFrequency = 1f;

        [SerializeField, Range(0.0f, 1.0f)]
        private float _modulationAmplitude = 0f;

        private Light2D _light;
        private float _initialIntensity = 1f;
        private bool _playInUnscaledTime = false;
        private float _time = 0f;

        public void Initialize(Light2D light, bool playInUnscaledTime)
        {
            _light = light;
            if (_light == null)
            {
                Debug.LogWarning("Light2D component is missing. LightIntensityController will not function properly.");
                return;
            }

            _initialIntensity = _light.intensity;
            _playInUnscaledTime = playInUnscaledTime;

            _light.intensity = 0f; // 初期化時点では光源を消灯しておく
        }

        public void Play()
        {
            _time = 0f;
        }

        private void Update()
        {
            if (_light == null)
                return;

            _time += _playInUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

            float curveTime = Mathf.Clamp01(_time / _curveDuration);
            float curveValue = _intensityCurve.Evaluate(curveTime);

            if (Mathf.Approximately(_modulationAmplitude, 0f))
            {
                _light.intensity = _initialIntensity * curveValue;
                return;
            }
            else
            {
                var noise = Mathf.PerlinNoise1D(_time * _modulationFrequency);
                _light.intensity = _initialIntensity * curveValue * (1f + (noise * 2f - 1f) * _modulationAmplitude);
            }
        }
    }
}
