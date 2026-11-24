using UnityEngine;
using static Constants;

public interface IPlayerState
{
    void Update(Vector2 inputDirection);
    void FixedUpdate(Vector2 inputDirection);
    void OnJumpInput();
    void Die(DeathReason deathReason);
    void Goal();
    void Freeze();
    void OnEnabled();
    void OnDisabled();

    bool IsAlive { get; }
    bool HasReachedGoal { get; }
    string GetAnimationName();
}
