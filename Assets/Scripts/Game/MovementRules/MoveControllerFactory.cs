using static Constants;

public static class MoveControllerFactory
{
    public static IMoveController Create(
        MovementRuleEffect rule,
        PlayersCollectionReadonlyAccess playersAccess
    )
    {
        switch (rule)
        {
            case MovementRuleEffect.Demo:
                return new DemoMoveController();
            case MovementRuleEffect.Reverse:
                return new ReverseMoveController(playersAccess);
            default:
                return new DemoMoveController();
        }
    }
}
