using UnityEngine;

public class AirState : PlayableState
{
    public AirState(IPlayerStateContext context, PlayerSounds sounds)
        : base(context, sounds) { }

    public override void OnMove(Vector2 inputDirection)
    {
        Context.MoveByInput(inputDirection);

        if (Context.IsGrounded())
            Context.ChangeState(new GroundState(Context, Sounds));
    }
}
