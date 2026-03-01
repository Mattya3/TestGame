using UnityEngine;

namespace REDACTED_PROJECT_NAME.Camera.DestinationSpecifier
{
    [System.Serializable]
    public class FrameGameObjects : IDestinationSpecifier
    {
        [SerializeField]
        private GameObject[] _targets;

        [SerializeField]
        private Vector2 _offset;

        public CameraTransform GetDestination()
        {
            if (_targets.Length == 0)
                return new CameraTransform(new Vector3(_offset.x, _offset.y, 0f), 1f);

            // Get the mean of all the target positions.
            var center = Vector3.zero;
            foreach (var go in _targets)
            {
                center += go.transform.position;
            }
            center /= _targets.Length;
            center.x += _offset.x;
            center.y += _offset.y;

            return new CameraTransform(center, 1f);
        }
    }
}