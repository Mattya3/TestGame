using UnityEngine;

namespace EffectsCompositeComponent
{
    [RequireComponent(typeof(ShakeEffectsPlayer))]
    public class PositionShakeEffect : MonoBehaviour, ITransformEffect
    {
        [SerializeField]
        private ShakeEffect _shakeEffect;

        private ShakeEffectsPlayer _shakeEffectsPlayer;
        private TransformOffsetController _transformOffsetController;

        public bool isEnabled => enabled;

        private void Awake()
        {
            _shakeEffectsPlayer = GetComponent<ShakeEffectsPlayer>();

            if (_shakeEffect == null)
            {
                Debug.LogError("ShakeEffect is not assigned in the inspector.");
            }
        }

        public void Initialize(TransformOffsetController transformOffsetController, bool playInUnscaledTime)
        {
            _transformOffsetController = transformOffsetController;
            if (_transformOffsetController == null)
            {
                Debug.LogError("TransformOffsetController is not assigned.");
            }

            if (playInUnscaledTime != _shakeEffect.UpdateMode.Equals(ShakeEffect.ShakeUpdateMode.UnscaledTime))
            {
                Debug.LogWarning("The playInUnscaledTime parameter does not match the ShakeEffect's UpdateMode. This may lead to unexpected behavior.");
            }
        }

        public void Play()
        {
            if (_shakeEffect == null)
                return;

            _shakeEffectsPlayer.Play(_shakeEffect);
        }

        private void LateUpdate()
        {
            var shakeOffset = _shakeEffectsPlayer.CurrentShakeOffset;
            _transformOffsetController.SetPositionOffset(shakeOffset);
        }
    }
}