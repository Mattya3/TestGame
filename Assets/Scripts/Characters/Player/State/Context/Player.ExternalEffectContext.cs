using UnityEngine;

public partial class Player
{
    // memo: 全部の外的要因をここで書かないといけないのが違和感
    private sealed class PlayerExternalEffectContext
        : IExternalEffectContext,
            IGravityEffectContext,
            IInputDirectionEffectContext
    {
        private readonly Player _player;
        private IExternalEffect _externalEffect;
        private bool _isEffectActive;

        public PlayerExternalEffectContext(Player player)
        {
            _player = player;
        }

        public void UpdateEffectState()
        {
            if (_externalEffect == null)
            {
                _isEffectActive = false;
                return;
            }

            bool shouldApply = _externalEffect.ShouldApply();
            if (!_isEffectActive && shouldApply)
            {
                _externalEffect.Apply();
            }
            else if (_isEffectActive && !shouldApply)
            {
                _externalEffect.Reset();
            }

            _isEffectActive = shouldApply;
        }

        public Vector2 GetMoveDirection(Vector2 inputDirection)
        {
            if (!_isEffectActive)
                return inputDirection;

            if (_externalEffect is IInputDirectionEffect inputDirectionEffect)
            {
                return inputDirectionEffect.ConvertInputDirection(inputDirection);
            }

            return inputDirection;
        }

        public void SetExternalEffect(IExternalEffect externalEffect)
        {
            _externalEffect?.Reset();
            _externalEffect = externalEffect;
            _isEffectActive = false;
        }

        public void SetGravityScale(float gravityScale)
        {
            _player._SetGravityScaleForExternalEffect(gravityScale);
        }

        public float GetDefaultGravityScale()
        {
            return _player._GetDefaultGravityScaleForExternalEffect();
        }

        public Vector2 ReverseHorizontalInput(Vector2 inputDirection)
        {
            return new Vector2(-inputDirection.x, inputDirection.y);
        }
    }
}
