using System;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class MovementRuleManager : MonoBehaviour
{
    [SerializeField]
    private MovementRuleEffect _movementRuleEffect;

    private IMoveController _moveController;

    public void Initialize(IReadOnlyList<Player> players)
    {
        _moveController = MoveControllerFactory.Create(_movementRuleEffect, players);
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
