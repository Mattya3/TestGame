using UnityEngine;
using static Constants;

namespace CharacterState
{
    public interface ICharacterState
    {
        void OnMove();
        void OnJump();
        void Die(DeathReason deathReason);
        void OnEnabled();
        void OnDisabled();
    }
}
