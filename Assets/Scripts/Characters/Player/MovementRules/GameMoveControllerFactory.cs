using System.Collections.Generic;
using static GameConstants;

public static class GameMoveControllerFactory
{
    public static IGameMoveController Create(MovementRuleEffect rule, List<Player> players)
    {
        switch (rule)
        {
            case MovementRuleEffect.Demo:
                return new DemoMoveController();
            case MovementRuleEffect.Reverse:
                return new ReverseMoveController(players.AsReadOnly());
            default:
                return new DemoMoveController();
        }
    }
}
