using UnityEngine;

namespace EffectsCompositeComponent
{
    [RequireComponent(typeof(ShakeEffectsPlayer))]
    public class PositionShakeEffect : MonoBehaviour, ITransformEffect
    {
        [SerializeField]
        private ShakeEffect _shakeEffect;

        [SerializeField]
        private bool _applyPlaybackSpeedRate = true;

        private ShakeEffectsPlayer _shakeEffectsPlayer;
        private TransformOffsetController _transformOffsetController;
        private bool _playInUnscaledTime = false;
        private float _playbackSpeedRate = 1.0f;

        public bool isEnabled => enabled;

        private void Awake()
        {
            _shakeEffectsPlayer = GetComponent<ShakeEffectsPlayer>();

            if (_shakeEffect == null)
            {
                Debug.LogError("ShakeEffect is not assigned in the inspector.");
                enabled = false;
                return;
            }
        }

        public void Initialize(
            TransformOffsetController transformOffsetController,
            bool playInUnscaledTime,
            float playbackSpeedRate
        )
        {
            _playInUnscaledTime = playInUnscaledTime;
            _playbackSpeedRate = playbackSpeedRate;

            _transformOffsetController = transformOffsetController;
            if (_transformOffsetController == null)
            {
                Debug.LogError("TransformOffsetController is not assigned.");
                enabled = false;
            }
        }

        public void Play()
        {
            if (_shakeEffect == null)
                return;

            _shakeEffectsPlayer.Play(
                _shakeEffect,
                _playInUnscaledTime,
                _applyPlaybackSpeedRate ? _playbackSpeedRate : 1.0f
            );
        }

        private void LateUpdate()
        {
            if (_shakeEffectsPlayer == null || _transformOffsetController == null)
                return;

            var shakeOffset = _shakeEffectsPlayer.CurrentShakeOffset;
            _transformOffsetController.SetPositionOffset(shakeOffset);
        }
    }
}
