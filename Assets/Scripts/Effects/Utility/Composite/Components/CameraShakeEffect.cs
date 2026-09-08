using UnityEngine;

namespace EffectsCompositeComponent
{
    public class CameraShakeEffect : MonoBehaviour, ICameraEffect
    {
        [SerializeField]
        private ShakeEffect _shakeEffect;

        private CameraMutableAccess _cameraAccess;

        public bool isEnabled => enabled;

        public void Initialize(CameraMutableAccess cameraAccess, bool playInUnscaledTime, float playSpeedRate)
        {
            _cameraAccess = cameraAccess;
            if (_cameraAccess == null)
            {
                Debug.LogError("Camera access is not initialized.");
            }
            if (_shakeEffect == null)
            {
                Debug.LogError("Shake effect is not assigned.");
            }
            else if ((_shakeEffect.UpdateMode == ShakeEffect.ShakeUpdateMode.UnscaledTime) != playInUnscaledTime)
            {
                Debug.LogWarning("Shake effect update mode does not match the playInUnscaledTime setting.");
            }
        }

        public void Play()
        {
            if (_cameraAccess == null)
                return;

            _cameraAccess.PlayShake(_shakeEffect);
        }
    }
}