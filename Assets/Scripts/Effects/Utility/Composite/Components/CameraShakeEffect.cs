using UnityEngine;

namespace EffectsCompositeComponent
{
    public class CameraShakeEffect : MonoBehaviour, ICameraEffect
    {
        [SerializeField]
        private ShakeEffect _shakeEffect;

        private CameraMutableAccess _cameraAccess;

        public bool isEnabled => enabled;

        public void Initialize(CameraMutableAccess cameraAccess, bool playInUnscaledTime)
        {
            _cameraAccess = cameraAccess;
            if (_cameraAccess == null)
            {
                Debug.LogError("Camera access is not initialized.");
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