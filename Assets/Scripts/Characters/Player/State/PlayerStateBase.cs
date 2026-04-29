using UnityEngine;
using static Constants;

public abstract class PlayerStateBase : IPlayerState
{
    protected PlayerStateBase(Player player, PlayerSounds sounds)
    {
        Player = player;
        Sounds = sounds;
    }

    protected Player Player { get; }
    protected PlayerSounds Sounds { get; }

    public virtual void OnMove(Vector2 inputDirection) { }

    public virtual void OnJump() { }

    public virtual void Die(DeathReason deathReason) { }

    public virtual void Goal() { }

    public virtual void OnEnabled() { }

    public virtual void OnDisabled() { }
}
