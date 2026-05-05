using UnityEngine;
using static Constants;

public partial class Player
{
    private sealed class PlayerStateContext : IPlayerStateContext
    {
        private readonly Player _player;
        private readonly EffectBehavior _defaultExternalEffectBehavior;
        private EffectBehavior _externalEffectBehavior;

        public PlayerStateContext(Player player)
        {
            _player = player;
            _defaultExternalEffectBehavior = new EffectBehavior(player);
            _externalEffectBehavior = _defaultExternalEffectBehavior;
        }

        Object IPlayerStateContext.LogContext => _player;

        public void SetExternalEffectBehavior(EffectBehavior behavior)
        {
            _externalEffectBehavior = behavior ?? _defaultExternalEffectBehavior;
        }

        public void ResetExternalEffectBehavior()
        {
            _externalEffectBehavior = _defaultExternalEffectBehavior;
        }

        void IPlayerStateContext.ChangeState(IPlayerState nextState)
        {
            _player._ChangeState(nextState);
        }

        bool IPlayerStateContext.IsGrounded()
        {
            return _player._IsGrounded();
        }


        void IPlayerStateContext.Freeze()
        {
            _player.Freeze();
        }

        void IPlayerStateContext.NotifyDied(DeathReason deathReason)
        {
            _player._NotifyDied(deathReason);
        }

        void IPlayerStateContext.NotifyGoalReached()
        {
            _player._NotifyGoalReached();
        }

        void IPlayerStateContext.MoveByInput(Vector2 inputDirection)
        {
            _externalEffectBehavior.MoveByInput(inputDirection);
        }

        bool IPlayerStateContext.TryJump()
        {
            return _externalEffectBehavior.TryJump();
        }
    }
}
