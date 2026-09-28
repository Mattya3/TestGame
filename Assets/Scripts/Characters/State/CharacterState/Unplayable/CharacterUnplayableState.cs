namespace CharacterState
{
    public abstract class CharacterUnplayableState : CharacterStateBase
    {
        protected CharacterUnplayableState(ICharacterStateContext context, PlayerSounds sounds)
            : base(context, sounds) { }
    }
}
