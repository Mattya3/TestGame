using UnityEngine;

public partial class Player
{
    private sealed class PlayerExternalEffectContext
    {
        private readonly Player _player;
        private bool _isEffectActive;

        public PlayerExternalEffectContext(Player player)
        {
            _player = player;
        }

        public void UpdateEffectState()
        {
            IExternalEffect externalEffect = _player._externalEffect;
            if (externalEffect == null)
            {
                _isEffectActive = false;
                return;
            }

            bool shouldApply = externalEffect.ShouldApply();
            if (!_isEffectActive && shouldApply)
            {
                externalEffect.Apply();
            }
            else if (_isEffectActive && !shouldApply)
            {
                externalEffect.Reset();
            }

            _isEffectActive = shouldApply;
        }

        public Vector2 GetMoveDirection(Vector2 inputDirection)
        {
            if (!_isEffectActive)
                return inputDirection;
            
            // memo: この書き方がすごい違和感, 解決策はないか
            if (_player._externalEffect is IInputDirectionEffect inputDirectionEffect)
            {
                return inputDirectionEffect.ConvertInputDirection(_player, inputDirection);
            }

            return inputDirection;
        }
    }
}
