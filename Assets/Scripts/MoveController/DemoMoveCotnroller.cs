using UnityEngine;

public class DemoMoveController : IGameMoveController
{
    public Vector2 ConvertInputDirection(Vector2 rawInput) => rawInput;
}
