using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using static GameConstants;

public class GameMoveController
{
    private readonly MovementRuleEffect _rule;
    private readonly ReadOnlyCollection<Player> _players;

    public GameMoveController(MovementRuleEffect rule, List<Player> players)
    {
        _rule = rule;
        _players = players.AsReadOnly();
    }

    public bool ShouldReverseInput()
    {
        switch (_rule)
        {
            case MovementRuleEffect.Demo:
                return false;
            case MovementRuleEffect.Reverse:
                return _players.Count(p => p.InputDirection.x != 0) >= MAX_PLAYERS;
            default:
                return false;
        }
    }
}
