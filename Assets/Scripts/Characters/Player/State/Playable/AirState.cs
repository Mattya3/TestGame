using UnityEngine;

public class AirState : PlayableState
{
    private static readonly string _EFFECT_KEY_LAND = "Land";

    public AirState(IPlayerStateContext context, PlayerSounds sounds)
        : base(context, sounds) { }

    public override void OnMove(Vector2 inputDirection)
    {
        Context.MoveByInput(inputDirection);

        if (Context.IsGrounded())
        {
            Context.PlayCharacterEffect(_EFFECT_KEY_LAND);
            Context.ChangeState(new GroundState(Context, Sounds));
        }
    }

    public override string GetAnimationName()
    {
        return Context.CurrentLinearVelocity.y > 0 ? "AirUp" : "AirDown";
    }
}
