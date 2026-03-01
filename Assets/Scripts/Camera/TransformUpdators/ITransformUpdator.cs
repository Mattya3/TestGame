using UnityEngine;

namespace REDACTED_PROJECT_NAME.Camera
{
    public interface ITransformUpdator
    {
        void LateUpdate(ref CameraTransform inoutTransform, in CameraTransform destTransform);
    }
}