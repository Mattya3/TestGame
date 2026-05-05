using UnityEngine;

public partial class Player
{
    public sealed class ReverseGravityBehavior : EffectBehavior
    {
        private float _originalGravityScale;

        public ReverseGravityBehavior(Player player)
            : base(player) { }

        public override void OnEnabled()
        {
            _originalGravityScale = Player._GetGravityScale();
            Player._SetGravityScale(-_originalGravityScale);
        }

        public override void OnDisabled()
        {
            Player._SetGravityScale(_originalGravityScale);
        }
    }
}
