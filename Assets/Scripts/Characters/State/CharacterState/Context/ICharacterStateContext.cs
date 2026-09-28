using UnityEngine;
using static Constants;

namespace CharacterState
{
    public interface ICharacterStateContext
    {
        Object LogContext { get; }
        void ChangeState(ICharacterState nextState);
        void Move();
        bool IsGrounded();
        bool TryJump();
        void Freeze();
        void NotifyDied(DeathReason deathReason);
        void NotifyGoalReached();
    }
}
