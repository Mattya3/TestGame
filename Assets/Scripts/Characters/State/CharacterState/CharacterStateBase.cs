using UnityEngine;

namespace CharacterState
{
    public abstract class CharacterStateBase : ICharacterState
    {
        protected CharacterStateBase(ICharacterStateContext context, PlayerSounds sounds)
        {
            Context = context;
            Sounds = sounds;
        }

        protected ICharacterStateContext Context { get; }
        protected PlayerSounds Sounds { get; }

        public virtual void OnMove() { }

        public virtual void OnJump() { }

        public virtual void Die(Constants.DeathReason deathReason) { }

        public virtual void OnEnabled() { }

        public virtual void OnDisabled() { }
    }
}
