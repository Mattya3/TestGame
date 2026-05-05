using UnityEngine;

public sealed class ReverseInputEffect : ExternalEffectBase
{
    public override Vector2 TransformInput(Player player, Vector2 inputDirection)
    {
        return new Vector2(-inputDirection.x, inputDirection.y);
    }
}
