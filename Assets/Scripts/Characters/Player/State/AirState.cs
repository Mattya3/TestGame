using UnityEngine;

public class AirState : PlayableState
{
    public AirState(Player player, PlayerSounds sounds)
        : base(player, sounds) { }

    public override void OnMove(Vector2 inputDirection)
    {
        Player.MoveByInput(inputDirection);

        if (Player.IsGrounded())
            Player.ChangeState(new GroundState(Player, Sounds));
    }
}
