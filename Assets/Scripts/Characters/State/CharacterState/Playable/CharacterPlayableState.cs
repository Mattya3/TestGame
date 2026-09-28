using static Constants;

namespace CharacterState
{
    public abstract class CharacterPlayableState : CharacterStateBase
    {
        protected CharacterPlayableState(ICharacterStateContext context, ICharacterSounds sounds)
            : base(context, sounds) { }

        public override void Die(DeathReason deathReason)
        {
            Context.ChangeState(new CharacterDeadState(Context, Sounds, deathReason));
        }
    }
}
