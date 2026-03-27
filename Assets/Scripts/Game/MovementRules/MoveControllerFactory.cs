using System.Collections.Generic;
using static Constants;

public static class MoveControllerFactory
{
    public static IMoveController Create(MovementRuleEffect rule, IReadOnlyList<Player> players)
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
