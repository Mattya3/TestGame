using UnityEngine;
using static Constants;

public abstract class PlayerStateBase : IPlayerState
{
    protected PlayerStateBase(Player player)
    {
        Player = player;
    }

    protected Player Player { get; }

    public virtual void OnMove(Vector2 inputDirection) { }

    public virtual void OnJump() { }

    public virtual void Die(DeathReason deathReason) { }

    public virtual void Goal() { }

    public virtual void OnEnabled() { }

    public virtual void OnDisabled() { }
}
