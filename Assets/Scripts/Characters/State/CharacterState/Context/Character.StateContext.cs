using UnityEngine;
using static Constants;

public partial class Character
{
    private sealed class CharacterStateContext : CharacterState.ICharacterStateContext
    {
        private readonly Character _character;

        public CharacterStateContext(Character character)
        {
            _character = character;
        }

        Object CharacterState.ICharacterStateContext.LogContext => _character;

        void CharacterState.ICharacterStateContext.ChangeState(
            CharacterState.ICharacterState nextState
        )
        {
            _character._ChangeState(nextState);
        }

        void CharacterState.ICharacterStateContext.Move()
        {
            _character._MoveCharacter();
        }

        bool CharacterState.ICharacterStateContext.IsGrounded()
        {
            return _character._groundDetector.IsGrounded();
        }

        bool CharacterState.ICharacterStateContext.TryJump()
        {
            if (!_character._groundDetector.IsGrounded())
                return false;

            _character._ApplyJump();
            return true;
        }

        void CharacterState.ICharacterStateContext.Freeze()
        {
            _character.Freeze();
        }

        void CharacterState.ICharacterStateContext.NotifyDied(DeathReason deathReason)
        {
            _character._NotifyDied(deathReason);
        }

        void CharacterState.ICharacterStateContext.NotifyGoalReached()
        {
            _character._NotifyGoalReached();
        }
    }
}
