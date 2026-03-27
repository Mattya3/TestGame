using System.Collections.Generic;
using UnityEngine;

public class ReverseMoveController : IMoveController
{
    private readonly IReadOnlyList<Player> _players;
    private readonly int _requiredCount;

    public ReverseMoveController(IReadOnlyList<Player> players)
    {
        _players = players;
        _requiredCount = players.Count;
    }

    public Vector2 ConvertInputDirection(Vector2 rawInput)
    {
        return _ShouldReverseInput() ? new Vector2(-rawInput.x, rawInput.y) : rawInput;
    }

    private bool _ShouldReverseInput()
    {
        int movingCount = 0;
        for (int i = 0; i < _players.Count; i++)
        {
            if (_players[i].InputDirection.x != 0)
                movingCount++;
        }
        return movingCount >= _requiredCount;
    }
}
