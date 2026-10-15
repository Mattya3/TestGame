using UnityEngine;
using UnityEngine.Serialization;

namespace EffectsCompositeComponent
{
    public class CameraShakeEffect : MonoBehaviour, ICameraEffect
    {
        [SerializeField]
        private ShakeEffect _shakeEffect;

        [SerializeField]
        [FormerlySerializedAs("_applyPlaySpeedRate")]
        private bool _applyPlaybackSpeedRate = true;

        private CameraMutableAccess _cameraAccess;
        private bool _playInUnscaledTime = false;
        private float _playbackSpeedRate = 1.0f;

        public bool isEnabled => enabled;

        public void Initialize(
            CameraMutableAccess cameraAccess,
            bool playInUnscaledTime,
            float playbackSpeedRate
        )
        {
            _playInUnscaledTime = playInUnscaledTime;
            _playbackSpeedRate = playbackSpeedRate;

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
                _applyPlaybackSpeedRate ? _playbackSpeedRate : 1.0f
            );
        }
    }
}
