using UnityEngine;

public partial class Player
{
    public sealed class ReverseInputBehavior : EffectBehavior
    {
        public ReverseInputBehavior(Player player)
            : base(player)
        {
        }

        public override void MoveByInput(Vector2 inputDirection)
        {
            Vector2 reversedInputDirection = new Vector2(-inputDirection.x, inputDirection.y);
            Player._MoveByInput(reversedInputDirection);
        }
    }
}
