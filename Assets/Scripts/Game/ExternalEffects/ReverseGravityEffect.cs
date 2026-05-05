using UnityEngine;

public sealed class ReverseGravityEffect : ExternalEffectBase
{
    public override float TransformGravityScale(Player player, float gravityScale)
    {
        return -gravityScale;
    }
}
