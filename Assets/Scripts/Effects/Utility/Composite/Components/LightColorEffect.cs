using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace EffectsCompositeComponent
{
    [RequireComponent(typeof(Light2D))]
    public class LightColorEffect : MonoBehaviour, ILightSourceEffect
    {
        [SerializeField]
        private Gradient _colorGradient = new Gradient();

        [SerializeField, Min(1e-3f)]
        private float _gradientDuration = 1f;

        private Light2D _light;
        private Color _initialColor = Color.white;
        private bool _playInUnscaledTime = false;
        private float _time = 0f;
        private bool _isPlaying = false;

        public bool isEnabled => enabled;

        public void Initialize(Light2D light, bool playInUnscaledTime)
        {
            _light = light;
            if (_light == null)
            {
                Debug.LogWarning("Light2D component is missing. LightColorController will not function properly.");
                return;
            }

            _initialColor = _light.color;
            _playInUnscaledTime = playInUnscaledTime;

            if (!enabled)
                return;
            _light.color = Color.black; // 初期化時点では光源を消灯しておく
        }

        public void Play()
        {
            _time = 0f;
            _isPlaying = true;
        }

        private void Update()
        {
            if (_light == null)
                return;

            if (!_isPlaying)
                return;

            _time += _playInUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

            float gradientTime = Mathf.Clamp01(_time / _gradientDuration);
            Color gradientColor = _colorGradient.Evaluate(gradientTime);
            _light.color = _initialColor * gradientColor;
        }
    }
}