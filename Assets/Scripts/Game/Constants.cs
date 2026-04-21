public static class Constants
{
    public static class Tags
    {
        public const string DEAD_ZONE = "DeadZone";
    }

    public static class Layers
    {
        public const string SOLID = "Solid";
        public const string PLAYER = "Player";
    }

    public static class AnimationTrigger
    {
        public const string OPENING = "Opening";
        public const string RESTART = "Restart";
        public const string FAILURE = "Failure";
        public const string SUCCESS = "Success";
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
