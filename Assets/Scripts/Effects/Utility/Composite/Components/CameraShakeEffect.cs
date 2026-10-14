using UnityEngine;

namespace EffectsCompositeComponent
{
    public class CameraShakeEffect : MonoBehaviour, ICameraEffect
    {
        [SerializeField]
        private ShakeEffect _shakeEffect;

        [SerializeField]
        private bool _applyPlaySpeedRate = true;

        private CameraMutableAccess _cameraAccess;
        private bool _playInUnscaledTime = false;
        private float _playSpeedRate = 1.0f;

        public bool isEnabled => enabled;

        public void Initialize(
            CameraMutableAccess cameraAccess,
            bool playInUnscaledTime,
            float playSpeedRate
        )
        {
            _playInUnscaledTime = playInUnscaledTime;
            _playSpeedRate = playSpeedRate;

            _cameraAccess = cameraAccess;
            if (_cameraAccess == null)
            {
                Debug.LogError("Camera access is not initialized.");
            }
            if (_shakeEffect == null)
            {
                Debug.LogError("Shake effect is not assigned.");
            }
        }

        public void Play()
        {
            if (_cameraAccess == null)
                return;

            _cameraAccess.PlayShake(
                _shakeEffect,
                _playInUnscaledTime,
                _applyPlaySpeedRate ? _playSpeedRate : 1.0f
            );
        }
    }
}
