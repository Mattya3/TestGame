using UnityEngine;

public sealed class PlayerInputDirectionEffectTarget : IInputDirectionEffectTarget
{
    public Vector2 ReverseHorizontalInput(Vector2 inputDirection)
    {
        return new Vector2(-inputDirection.x, inputDirection.y);
    }
}
