using UnityEngine;

public class GroundState : PlayableState
{
    public GroundState(Player player, PlayerSounds sounds)
        : base(player, sounds) { }

    public override void OnMove(Vector2 inputDirection)
    {
        if (!Player.IsGrounded())
        {
            Player.ChangeState(new AirState(Player, Sounds));
            return;
        }

        Player.MoveByInput(inputDirection);
    }

    public override void OnJump()
    {
        if (!Player.TryJump())
            return;

        Sounds.OnJump();
        Player.ChangeState(new AirState(Player, Sounds));
    }

    public override void OnEnabled()
    {
        if (Player.PreviousState is AirState)
            Sounds.OnLand();
    }
}
