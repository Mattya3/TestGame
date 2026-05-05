using UnityEngine;

public partial class Player
{
    public class EffectBehavior
    {
        protected readonly Player Player;

        public EffectBehavior(Player player)
        {
            Player = player;
        }

        public virtual void MoveByInput(Vector2 inputDirection)
        {
            Player._MoveByInput(inputDirection);
        }

        public virtual bool TryJump()
        {
            return Player._TryJump();
        }

        public virtual void OnEnabled() { }

        public virtual void OnDisabled() { }
    }
}
