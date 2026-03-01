using UnityEngine;

namespace REDACTED_PROJECT_NAME.Camera
{
    public struct CameraTransform
    {
        public Vector3 _position { get; set; }
        public float _scale { get; set; }

        public CameraTransform(Vector3 position, float scale)
        {
            _position = position;
            _scale = scale;
        }
        public CameraTransform(UnityEngine.Transform transform)
        {
            _position = transform.position;
            _scale = transform.localScale.x; // Assuming uniform scaling
        }

        public void ApplyTo(Transform transform)
        {
            transform.position = new Vector3(_position.x, _position.y, transform.position.z); // Keep original z position
            transform.localScale = new Vector3(_scale, _scale, 1f);
        }
    }
}