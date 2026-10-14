using UnityEngine;

namespace EffectsCompositeComponent
{
    [RequireComponent(typeof(ShakeEffectsPlayer))]
    public class PositionShakeEffect : MonoBehaviour, ITransformEffect
    {
        [SerializeField]
        private ShakeEffect _shakeEffect;

        [SerializeField]
        private bool _applyPlaySpeedRate = true;

        private ShakeEffectsPlayer _shakeEffectsPlayer;
        private TransformOffsetController _transformOffsetController;
        private bool _playInUnscaledTime = false;
        private float _playSpeedRate = 1.0f;

        public bool isEnabled => enabled;

        private void Awake()
        {
            _shakeEffectsPlayer = GetComponent<ShakeEffectsPlayer>();

            if (_shakeEffect == null)
            {
                Debug.LogError("ShakeEffect is not assigned in the inspector.");
            }
        }

        public void Initialize(
            TransformOffsetController transformOffsetController,
            bool playInUnscaledTime,
            float playSpeedRate
        )
        {
            _playInUnscaledTime = playInUnscaledTime;
            _playSpeedRate = playSpeedRate;

            _transformOffsetController = transformOffsetController;
            if (_transformOffsetController == null)
            {
                Debug.LogError("TransformOffsetController is not assigned.");
            }

            if (_shakeEffect == null)
                return;
        }

        public void Play()
        {
            if (_shakeEffect == null)
                return;

            _shakeEffectsPlayer.Play(
                _shakeEffect,
                _playInUnscaledTime,
                _applyPlaySpeedRate ? _playSpeedRate : 1.0f
            );
        }

        private void LateUpdate()
        {
            var shakeOffset = _shakeEffectsPlayer.CurrentShakeOffset;
            _transformOffsetController.SetPositionOffset(shakeOffset);
        }
    }
}
