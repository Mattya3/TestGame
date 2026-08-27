using UnityEngine;

public sealed class PlayerInputDirectionEffectContext : IInputDirectionEffectContext
{
    public Vector2 ReverseHorizontalInput(Vector2 inputDirection)
    {
        return new Vector2(-inputDirection.x, inputDirection.y);
    }
}
