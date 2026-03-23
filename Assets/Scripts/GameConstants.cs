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
    
    /// <summary>
    /// ゲームのルールを定義します。
    /// 今後ステージ毎にルールをいじる場合に備えたenum,(操作判定の以外の部分について、例えば反重力とか)
    /// </summary>
    public enum MovementRuleEffect
    {
        Demo, //デモ用, 入力反転が起こらない
        Reverse, //通常用, 入力反転が起こる
    }
}
