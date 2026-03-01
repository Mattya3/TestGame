using UnityEngine;

namespace REDACTED_PROJECT_NAME.Camera
{
    public interface IConstraint
    {
        void ApplyTo(ref CameraTransform transform);
    }
}