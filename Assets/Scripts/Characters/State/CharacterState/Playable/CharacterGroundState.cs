using UnityEngine;

namespace CharacterState
{
    public sealed class CharacterGroundState : CharacterPlayableState
    {
        public CharacterGroundState(ICharacterStateContext context, ICharacterSounds sounds)
            : base(context, sounds) { }

        public override void OnMove()
        {
            if (!Context.IsGrounded())
            {
                Context.ChangeState(new CharacterAirState(Context, Sounds));
                return;
            }

            Context.Move();
        }

        public override void OnJump()
        {
            if (!Context.TryJump())
                return;

            Sounds.OnJump();
            Context.ChangeState(new CharacterAirState(Context, Sounds));
        }
    }
}
