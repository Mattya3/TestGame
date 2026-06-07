using UnityEngine;

public interface IInputDirectionEffect
{
    Vector2 ConvertInputDirection(Player player, Vector2 inputDirection);
}
