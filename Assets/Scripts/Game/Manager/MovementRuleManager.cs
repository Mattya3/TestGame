using UnityEngine;
using static GameConstants;
using System.Collections.Generic;
using System;

public class MovementRuleManager : MonoBehaviour
{
    [SerializeField]
    private MovementRuleEffect _movementRuleEffect;

    private IGameMoveController _moveController;

    public void Initialize(List<Player> players)
    {
        _moveController = GameMoveControllerFactory.Create(_movementRuleEffect, players);
    }

    public Vector2 ConvertInputDirection(Vector2 input)
    {        
        return _moveController.ConvertInputDirection(input);
    }
}
