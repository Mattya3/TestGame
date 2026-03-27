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
        foreach (var player in players)
        {
            _ApplyNewRule(player);
        }
    }

    private void _ApplyNewRule(Player player)
    {
        player.MoveController = _moveController;
    }
}
