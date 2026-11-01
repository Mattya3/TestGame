using UnityEngine;
using static Constants;

public interface IPlayerState
{
    void OnMove(Vector2 inputDirection);
    void OnJump();
    void Die(DeathReason deathReason);
    void Goal();
    void OnEnabled();
    void OnDisabled();
}
