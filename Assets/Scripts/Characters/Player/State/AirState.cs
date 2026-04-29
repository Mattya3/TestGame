using UnityEngine;

public class AirState : PlayableState
{
    public AirState(Player player)
        : base(player) { }

    public override void OnMove(Vector2 inputDirection)
    {
        Player.MoveByInput(inputDirection);

        if (Player.IsGrounded())
            Player.ChangeState(new GroundState(Player));
    }
}
