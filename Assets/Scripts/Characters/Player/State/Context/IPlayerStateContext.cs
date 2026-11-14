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
    void SetAnimatorUpdateMode(AnimatorUpdateMode updateMode);

    Vector2 CurrentInputDirection { get; }
    Vector2 CurrentLinearVelocity { get; }

    Character.FacingDirectionType FacingDirection { get; set; }
    void SetFacingDirectionByVector(Vector2 direction)
    {
        if (direction.x > 0)
            FacingDirection = Character.FacingDirectionType.Right;
        else if (direction.x < 0)
            FacingDirection = Character.FacingDirectionType.Left;
    }
}
