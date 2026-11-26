using UnityEngine;
using static Constants;

public partial class Player
{
    private sealed class StateContext : IPlayerStateContext
    {
        private readonly Player _player;

        public StateContext(Player player)
        {
            _player = player;
        }

        Object IPlayerStateContext.LogContext => _player;

        void IPlayerStateContext.ChangeState(IPlayerState nextState)
        {
            _player._ChangeState(nextState);
        }

        void IPlayerStateContext.MoveByInput(Vector2 inputDirection)
        {
            _player._MoveByInput(inputDirection);
        }

        bool IPlayerStateContext.IsGrounded()
        {
            return _player.IsGrounded();
        }

        bool IPlayerStateContext.TryJump()
        {
            return _player._TryJump();
        }

        void IPlayerStateContext.FreezePhysics()
        {
            _player.FreezePhysics();
        }

        void IPlayerStateContext.NotifyDied(DeathReason deathReason)
        {
            _player._NotifyDied(deathReason);
        }

        void IPlayerStateContext.NotifyGoalReached()
        {
            _player._NotifyGoalReached();
        }

        void IPlayerStateContext.SetAnimatorSpeed(float speed)
        {
            _player.SetAnimatorSpeed(speed);
        }

        void IPlayerStateContext.SetAnimatorUpdateMode(AnimatorUpdateMode updateMode)
        {
            _player.SetAnimatorUpdateMode(updateMode);
        }

        void IPlayerStateContext.CompleteCurrentAnimation()
        {
            _player.CompleteCurrentAnimation();
        }

        void IPlayerStateContext.PlayCharacterEffect(string effectKey)
        {
            _player.PlayCharacterEffect(effectKey);
        }

        void IPlayerStateContext.StopCharacterEffect(string effectKey)
        {
            _player.StopCharacterEffect(effectKey);
        }

        Vector2 IPlayerStateContext.CurrentInputDirection => _player._inputDirection;
        Vector2 IPlayerStateContext.CurrentLinearVelocity => _player.CurrentLinearVelocity;

        FacingDirectionType IPlayerStateContext.FacingDirection
        {
            get => _player.FacingDirection;
            set => _player.FacingDirection = value;
        }
    }
}
