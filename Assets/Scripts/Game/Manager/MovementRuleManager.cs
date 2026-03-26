using System;
using System.Collections.Generic;
using UnityEngine;
using static GameConstants;

public class MovementRuleManager : MonoBehaviour
{
    [SerializeField]
    private MovementRuleEffect _movementRuleEffect;

    private IGameMoveController _moveController;

    public void Initialize(IReadOnlyList<Player> players)
    {
        _moveController = GameMoveControllerFactory.Create(_movementRuleEffect, players);
    }

    public Vector2 ConvertInputDirection(Vector2 input)
    {
        return _moveController.ConvertInputDirection(input);
    }
}
