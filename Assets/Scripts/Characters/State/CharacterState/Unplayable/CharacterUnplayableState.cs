namespace CharacterState
{
    public abstract class CharacterUnplayableState : CharacterStateBase
    {
        protected CharacterUnplayableState(ICharacterStateContext context, ICharacterSounds sounds)
            : base(context, sounds) { }
    }
}
