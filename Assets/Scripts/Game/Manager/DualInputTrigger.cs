using System;
using System.Collections.Generic;
using UnityEngine;

public class DualInputTrigger
{
    public event Action<bool> OnDualInputStateChanged;

    private IReadOnlyList<Player> _players;
    private bool _isDualInputActive;

    public DualInputTrigger(IReadOnlyList<Player> players)
    {
        _players = players;
        foreach (var player in players)
        {
            player.OnInputDirectionChanged += _HandleInputChanged;
        }
        _HandleInputChanged(null, Vector2.zero);
    }

    private void _HandleInputChanged(Player _, Vector2 __)
    {
        bool nextState = (_players[0].InputDirection.x != 0f && _players[1].InputDirection.x != 0f);
        if (nextState == _isDualInputActive) return;

        _isDualInputActive = nextState;
        OnDualInputStateChanged?.Invoke(_isDualInputActive);
    }
}
