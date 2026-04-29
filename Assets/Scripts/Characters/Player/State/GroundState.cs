using UnityEngine;

public class GroundState : PlayableState
{
    public GroundState(Player player)
        : base(player) { }

    public override void OnMove(Vector2 inputDirection)
    {
        if (!Player.IsGrounded())
        {
            Player.ChangeState(new AirState(Player));
            return;
        }

        Player.MoveByInput(inputDirection);
    }

    public override void OnJump()
    {
        if (!Player.TryJump())
            return;

        Player.PlayJumpSound();
        Player.ChangeState(new AirState(Player));
    }

    public override void OnEnabled()
    {
        if (Player.PreviousState is AirState)
            Player.PlayLandSound();
    }
}
