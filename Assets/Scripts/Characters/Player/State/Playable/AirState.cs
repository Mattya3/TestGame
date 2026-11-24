using UnityEngine;

public class AirState : PlayableState
{
    private static readonly string _EFFECT_KEY_LAND = "Land";

    public AirState(IPlayerStateContext context)
        : base(context) { }

    public override void Update(Vector2 inputDirection)
    {
        base.Update(inputDirection);

        Context.MoveByInput(inputDirection);

        if (Context.IsGrounded())
        {
            Context.PlayCharacterEffect(_EFFECT_KEY_LAND);
            Context.ChangeState(new GroundState(Context));
        }
    }

    public override string GetAnimationName()
    {
        return Context.CurrentLinearVelocity.y > 0 ? "AirUp" : "AirDown";
    }
}
