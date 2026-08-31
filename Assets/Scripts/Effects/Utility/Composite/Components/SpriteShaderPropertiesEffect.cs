using System;
using UnityEngine;

namespace EffectsCompositeComponent
{
    public class SpriteShaderPropertiesEffect : MonoBehaviour, IRendererEffect
    {
        [Serializable]
        private abstract class PropertyBase
        {
            [SerializeField]
            private string propertyName;

            [SerializeField, Min(1e-3f)]
            private float duration = 1f;

            private int propertyID = -1;

            public void Initialize()
            {
                propertyID = Shader.PropertyToID(propertyName);
            }

            public string PropertyName => propertyName;

            public int PropertyID => propertyID;

            public float Duration => duration;
        }

        [Serializable]
        private class FloatProperty : PropertyBase
        {
            [SerializeField]
            private AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

            public float Evaluate(float time)
            {
                return curve.Evaluate(time / Duration);
            }
        }

        [Serializable]
        private class ColorProperty : PropertyBase
        {
            [SerializeField]
            private Gradient gradient = new Gradient();

            public Color Evaluate(float time)
            {
                return gradient.Evaluate(time / Duration);
            }
        }

        [SerializeField]
        private FloatProperty[] floatProperties = Array.Empty<FloatProperty>();

        [SerializeField]
        private ColorProperty[] colorProperties = Array.Empty<ColorProperty>();

        private Renderer _renderer;
        private MaterialPropertyBlock _materialPropertyBlock;
        private bool _playInUnscaledTime = false;
        private float _time = 0f;
        private bool _isPlaying = false;

        public bool isEnabled => enabled;

        public void Initialize(Renderer renderer, bool playInUnscaledTime)
        {
            _renderer = renderer;
            if (_renderer == null)
            {
                Debug.LogWarning("Renderer component is missing. ShaderPropertiesEffect will not function properly.");
                return;
            }
            _materialPropertyBlock = new MaterialPropertyBlock();

            _playInUnscaledTime = playInUnscaledTime;

            foreach (var floatProp in floatProperties)
            {
                floatProp.Initialize();
                if (!_renderer.sharedMaterial.HasProperty(floatProp.PropertyID))
                {
                    Debug.LogWarning($"Property '{floatProp.PropertyName}' not found in the material. Please ensure the property name is correct and exists in the shader.");
                }
            }
            foreach (var colorProp in colorProperties)
            {
                colorProp.Initialize();
                if (!_renderer.sharedMaterial.HasProperty(colorProp.PropertyID))
                {
                    Debug.LogWarning($"Property '{colorProp.PropertyName}' not found in the material. Please ensure the property name is correct and exists in the shader.");
                }
            }
        }

        public void Play()
        {
            _time = 0f;
            _isPlaying = true;
        }

        private void Update()
        {
            if (!_renderer)
                return;

            if (!_isPlaying)
                return;

            _time += _playInUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

            _renderer.GetPropertyBlock(_materialPropertyBlock);
            foreach (var floatProp in floatProperties)
            {
                float value = floatProp.Evaluate(_time);
                _materialPropertyBlock.SetFloat(floatProp.PropertyID, value);
            }
            foreach (var colorProp in colorProperties)
            {
                Color value = colorProp.Evaluate(_time);
                _materialPropertyBlock.SetColor(colorProp.PropertyID, value);
            }
            _renderer.SetPropertyBlock(_materialPropertyBlock);
        }
    }
}