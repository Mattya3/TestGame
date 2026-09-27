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

    public const int PLAYER_COUNT = 2;

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

    public enum ExternalEffectType
    {
        None,
        ReverseInput,
        ReverseGravity,
        StopVerticalMovement,
    }

    public enum GameEvent
    {
        GamePlayStart = 0,
        Failure = 1,
        Success = 2,
        SceneEnd = 3,
    }
}
