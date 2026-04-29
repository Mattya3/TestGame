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

        bool IPlayerStateContext.PreviousStateIsAirState => _player._previousState is AirState;

        Object IPlayerStateContext.LogContext => _player;

        void IPlayerStateContext.ChangeState(IPlayerState nextState)
        {
            _player.ChangeStateInternal(nextState);
        }

        void IPlayerStateContext.MoveByInput(Vector2 inputDirection)
        {
            _player.MoveByInputInternal(inputDirection);
        }

        bool IPlayerStateContext.IsGrounded()
        {
            return _player.IsGroundedInternal();
        }

        bool IPlayerStateContext.TryJump()
        {
            return _player.TryJumpInternal();
        }

        void IPlayerStateContext.Freeze()
        {
            _player.Freeze();
        }

        void IPlayerStateContext.NotifyDied(DeathReason deathReason)
        {
            _player.NotifyDiedInternal(deathReason);
        }

        void IPlayerStateContext.NotifyGoalReached()
        {
            _player.NotifyGoalReachedInternal();
        }
    }
}
