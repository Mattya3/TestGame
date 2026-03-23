using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using static GameConstants;

public class GameMoveController
{
    private readonly MovementRuleEffect _rule;
    private readonly ReadOnlyCollection<Player> _players;

    private int _requiredPlayersForReverse;

    public GameMoveController(MovementRuleEffect rule, List<Player> players)
    {
        _rule = rule;
        _players = players.AsReadOnly();
        _requiredPlayersForReverse = _players.Count;
    }

    public bool ShouldReverseInput()
    {
        switch (_rule)
        {
            case MovementRuleEffect.Demo:
                return false;
            case MovementRuleEffect.Reverse:
                int movingPlayerCount = 0;
                for (int i = 0; i < _players.Count; i++)
                {
                    if (_players[i].InputDirection.x != 0)
                    {
                        movingPlayerCount++;
                    }
                }
                return movingPlayerCount >= _requiredPlayersForReverse;
            default:
                return false;
        }
    }
}
