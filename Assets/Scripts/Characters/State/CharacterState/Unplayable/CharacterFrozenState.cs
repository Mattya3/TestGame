using UnityEngine;
using static Constants;

namespace CharacterState
{
    public sealed class CharacterFrozenState : CharacterUnplayableState
    {
        public CharacterFrozenState(ICharacterStateContext context, ICharacterSounds sounds)
            : base(context, sounds) { }

        public override void OnEnabled()
        {
            Context.Freeze();
        }

        public override void Die(DeathReason deathReason)
        {
            Debug.LogWarning("Die() was called while in CharacterFrozenState.", Context.LogContext);
        }
    }
}
