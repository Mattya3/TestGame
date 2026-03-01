using UnityEngine;

namespace REDACTED_PROJECT_NAME.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraController2 : MonoBehaviour
    {
        [SerializeReference]
        private IDestinationSpecifier _destinationSpecifier = new DestinationSpecifier.FrameGameObjects();

        [SerializeReference]
        private ITransformUpdator _transformUpdator = new TransformUpdators.SmoothUpdator();

        // Awake is called when the script instance is being loaded
        void Awake()
        {
            if (_destinationSpecifier == null)
            {
                Debug.LogError("ƒJƒƒ‰‚ÌdestinationSpecifier‚ªİ’è‚³‚ê‚Ä‚¢‚Ü‚¹‚ñ", this);
                enabled = false;
            }
            if (_transformUpdator == null)
            {
                Debug.LogError("ƒJƒƒ‰‚ÌtransformUpdator‚ªİ’è‚³‚ê‚Ä‚¢‚Ü‚¹‚ñ", this);
                enabled = false;
            }
        }

        // LateUpdate is called after all Update functions have been called
        void LateUpdate()
        {
            var updatedTransform = new CameraTransform(transform);

            // specity destination
            var dest = _destinationSpecifier.GetDestination();

            // update transform
            _transformUpdator.LateUpdate(ref updatedTransform, dest);

            updatedTransform.ApplyTo(transform);
        }
    }
}