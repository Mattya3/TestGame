public static class GameConstants
{
    public static class Tags
    {
        public const string DEAD_ZONE = "DeadZone";
    }

    public static class Layers
    {
        public const string SOLID = "Solid";
        public const string CHARACTER = "Character";
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

    public enum MovementRuleEffect
    {
        Demo, //デモ用, 入力反転が起こらない
        Reverse, //通常用, 入力反転が起こる
    }

    public enum GameEvent
    {
        Failure,
        Success,
    }
}
