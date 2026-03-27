using System.Collections.Generic;
using static GameConstants;

public static class GameMoveControllerFactory
{
    public static IGameMoveController Create(MovementRuleEffect rule, IReadOnlyList<Player> players)
    {
        switch (rule)
        {
            case MovementRuleEffect.Demo:
                return new DemoMoveController();
            case MovementRuleEffect.Reverse:
                return new ReverseMoveController(players);
            default:
                return new DemoMoveController();
        }
    }
}
