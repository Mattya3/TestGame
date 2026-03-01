using UnityEngine;

namespace REDACTED_PROJECT_NAME.Camera
{
    public interface IDestinationSpecifier
    {
        CameraTransform GetDestination();
    }
}