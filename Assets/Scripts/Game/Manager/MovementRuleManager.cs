using System;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class MovementRuleManager : MonoBehaviour
{
    [SerializeField]
    private MovementRuleEffect _movementRuleEffect;

    private IMoveController _moveController;
    private IReadOnlyList<Player> _players;
    private bool _isDualInputActive;

    public event Action<bool> OnDualInputChanged;

    public void Initialize(IReadOnlyList<Player> players)
    {
        _players = players;
        _moveController = MoveControllerFactory.Create(_movementRuleEffect, players);
        foreach (var player in players)
        {
            _ApplyNewRule(player);
            player.OnInputDirectionChanged += _HandleDualInputChanged;
        }

        _UpdateDualInputState();
    }

    private void _ApplyNewRule(Player player)
    {
        player.MoveController = _moveController;
    }

    private void _HandleDualInputChanged(Player _, Vector2 __)
    {
        _UpdateDualInputState();
    }

    private void _UpdateDualInputState()
    {
        bool nextState = _HasDualInput();
        if (nextState == _isDualInputActive)
            return;

        _isDualInputActive = nextState;
        Debug.Log("同時入力がはっか");
        OnDualInputChanged?.Invoke(_isDualInputActive);
    }

    private bool _HasDualInput()
    {
        return _players[0].InputDirection.x != 0f
            && _players[1].InputDirection.x != 0f;
    }
}
