using UnityEngine;

namespace EffectsCompositeComponent
{
    public class SpriteColorEffect : MonoBehaviour, IRendererEffect
    {
        [SerializeField]
        private Gradient _colorGradient = new Gradient();

        [SerializeField, Min(1e-3f)]
        private float _gradientDuration = 1f;

        private SpriteRenderer _renderer;
        private Color _initialColor = Color.white;
        private bool _playInUnscaledTime = false;
        private float _time = 0f;
        private bool _isPlaying = false;

        public bool isEnabled => enabled;

        public void Initialize(Renderer renderer, bool playInUnscaledTime)
        {
            _renderer = renderer as SpriteRenderer;
            if (_renderer == null)
            {
                Debug.LogWarning("Renderer component is missing, or not a SpriteRenderer. SpriteColorEffect will not function properly.");
                return;
            }
            _initialColor = _renderer.material.color;
            _playInUnscaledTime = playInUnscaledTime;
        }

        public void Play()
        {
            _time = 0f;
            _isPlaying = true;
        }

        private void LateUpdate()
        {
            if (_renderer == null)
                return;

            if (!_isPlaying)
                return;

            _time += _playInUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

            float gradientTime = Mathf.Clamp01(_time / _gradientDuration);
            Color gradientColor = _colorGradient.Evaluate(gradientTime);
            _renderer.color = _initialColor * gradientColor;
        }
    }
}