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
            return _player._IsGrounded();
        }

        bool IPlayerStateContext.TryJump()
        {
            return _player._TryJump();
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
    }
}
