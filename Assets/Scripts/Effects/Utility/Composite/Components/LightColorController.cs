using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace EffectsCompositeComponent
{
    public class LightColorController : MonoBehaviour, ILightSourceController
    {
        [SerializeField]
        private Gradient _colorGradient = new Gradient();

        [SerializeField, Min(1e-3f)]
        private float _gradientDuration = 1f;

        [SerializeField]
        private bool _playInUnscaledTime = false;

        private Light2D _light;
        private Color _initialColor = Color.white;
        private float _time = 0f;

        public void Initialize(Light2D light)
        {
            _light = light;
            _initialColor = _light.color;
            _light.color = Color.black; // 初期化時点では光源を消灯しておく
        }

        public void Play()
        {
            _time = 0f;
        }

        private void Update()
        {
            _time += _playInUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

            float gradientTime = Mathf.Clamp01(_time / _gradientDuration);
            Color gradientColor = _colorGradient.Evaluate(gradientTime);
            _light.color = _initialColor * gradientColor;
        }
    }
}