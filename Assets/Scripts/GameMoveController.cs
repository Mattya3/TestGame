using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameConstants;

public class GameMoveController
{
    private readonly MovementRuleEffect _rule;

    public GameMoveController(MovementRuleEffect rule)
    {
        _rule = rule;
    }

    public bool ShouldReverseInput(IReadOnlyList<Player> players)
    {
        switch (_rule)
        {
            case MovementRuleEffect.Demo:
                return false;
            case MovementRuleEffect.Normal:
                return players.Count(p => p.InputDirection.x != 0) >= MAX_PLAYERS;
            default:
                return false;
        }
    }
}
