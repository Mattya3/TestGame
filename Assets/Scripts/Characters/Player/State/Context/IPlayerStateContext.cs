using UnityEngine;
using static Constants;

public interface IPlayerStateContext
{
    Object LogContext { get; }
    void ChangeState(IPlayerState nextState);
    void MoveByInput(Vector2 inputDirection);
    bool IsGrounded();
    bool TryJump();
    void Freeze();
    void NotifyDied(DeathReason deathReason);
    void NotifyGoalReached();

    Vector2 CurrentInputDirection { get; }
    Vector2 CurrentLinearVelocity { get; }
}
