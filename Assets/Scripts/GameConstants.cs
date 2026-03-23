public static class GameConstants
{
    public static class Tags
    {
        public const string DEAD_ZONE = "DeadZone";
    }

    public static class Layers
    {
        public const string SOLID = "Solid";
    }

    public static class AnimationTrigger
    {
        public const string PLAYER_DEATH = "PlayerDeath";
        public const string ALL_PLAYERS_GOAL = "AllPlayersGoal";
    }

    public enum DeathReason
    {
        Fall,
        DeadZone,
    }

    public enum GameEvent
    {
        Failure,
        Success,
    }
}
