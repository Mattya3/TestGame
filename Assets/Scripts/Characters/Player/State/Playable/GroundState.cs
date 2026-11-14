using UnityEngine;

public class GroundState : PlayableState
{
    private static readonly string _EFFECT_KEY_JUMP = "Jump";

    public GroundState(IPlayerStateContext context, PlayerSounds sounds)
        : base(context, sounds) { }

    public override void OnMove(Vector2 inputDirection)
    {
        if (!Context.IsGrounded())
        {
            Context.ChangeState(new AirState(Context, Sounds));
            return;
        }

        Context.MoveByInput(inputDirection);
        Context.SetFacingDirectionByVector(inputDirection);
    }

    public override void OnJump()
    {
        if (!Context.TryJump())
            return;

        Context.PlayCharacterEffect(_EFFECT_KEY_JUMP);
        Context.ChangeState(new AirState(Context, Sounds));
    }

    public override string GetAnimationName()
    {
        return Mathf.Approximately(Context.CurrentInputDirection.x, 0) ? "Idle" : "Run";
    }
}
