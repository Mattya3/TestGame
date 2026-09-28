using UnityEngine;

namespace CharacterState
{
    public sealed class CharacterAirState : CharacterPlayableState
    {
        public CharacterAirState(ICharacterStateContext context, PlayerSounds sounds)
            : base(context, sounds) { }

        public override void OnMove()
        {
            Context.Move();

            if (Context.IsGrounded())
            {
                Sounds.OnLand();
                Context.ChangeState(new CharacterGroundState(Context, Sounds));
            }
        }
    }
}
