using UnityEngine;
using static Constants;

namespace CharacterState
{
    public sealed class CharacterDeadState : CharacterUnplayableState
    {
        private readonly DeathReason _deathReason;

        public CharacterDeadState(
            ICharacterStateContext context,
            PlayerSounds sounds,
            DeathReason deathReason
        )
            : base(context, sounds)
        {
            _deathReason = deathReason;
        }

        public override void OnEnabled()
        {
            Context.NotifyDied(_deathReason);
            Context.Freeze();
            Sounds.OnDeath();
        }

        public override void Die(DeathReason deathReason)
        {
            Debug.LogWarning("Die() was called while in CharacterDeadState.", Context.LogContext);
        }
    }
}
