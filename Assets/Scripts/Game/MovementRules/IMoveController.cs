using UnityEngine;

public interface IMoveController
{
    Vector2 ConvertInputDirection(Vector2 rawInput);
}
