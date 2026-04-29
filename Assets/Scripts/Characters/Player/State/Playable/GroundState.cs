using UnityEngine;

public class GroundState : PlayableState
{
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
    }

    public override void OnJump()
    {
        if (!Context.TryJump())
            return;

        Sounds.OnJump();
        Context.ChangeState(new AirState(Context, Sounds));
    }

    public override void OnEnabled()
    {
        if (Context.PreviousStateIsAirState)
            Sounds.OnLand();
    }
}
