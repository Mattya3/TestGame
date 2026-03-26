using UnityEngine;

public interface IGameMoveController
{
    Vector2 ConvertInputDirection(Vector2 rawInput);
}
