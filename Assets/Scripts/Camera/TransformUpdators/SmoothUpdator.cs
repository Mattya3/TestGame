using UnityEngine;

namespace REDACTED_PROJECT_NAME.Camera.TransformUpdators
{
    [System.Serializable]
    public class SmoothUpdator : ITransformUpdator
    {
        [SerializeField]
        private float _smoothTime = 0.5f;

        private Vector3 _positionVelocity = Vector3.zero;
        private float _scaleVelocity = 0f;

        public void LateUpdate(ref CameraTransform inoutTransform, in CameraTransform destTransform)
        {
            inoutTransform._position = Vector3.SmoothDamp(inoutTransform._position, destTransform._position, ref _positionVelocity, _smoothTime);
            inoutTransform._scale = Mathf.SmoothDamp(inoutTransform._scale, destTransform._scale, ref _scaleVelocity, _smoothTime);
        }
    }
}