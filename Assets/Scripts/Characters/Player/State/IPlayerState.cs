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

public abstract class PlayerStateBase : IPlayerState
{
    protected PlayerStateBase(Player player)
    {
        _player = player;
    }

    protected Player _player;

    public virtual void OnMove(Vector2 inputDirection) { }

    public virtual void OnJump() { }

    public virtual void Die(DeathReason deathReason) { }

    public virtual void Goal() { }

    public virtual void OnEnabled() { }

    public virtual void OnDisabled() { }
}

public abstract class PlayableState : PlayerStateBase
{
    protected PlayableState(Player player)
        : base(player) { }
}

public abstract class UnplayableState : PlayerStateBase
{
    protected UnplayableState(Player player)
        : base(player) { }
}

public class GroundState : PlayableState
{
    public GroundState(Player player)
        : base(player) { }
}

public class AirState : PlayableState
{
    public AirState(Player player)
        : base(player) { }
}

public class FrozenState : UnplayableState
{
    public FrozenState(Player player)
        : base(player) { }
}

public class DeadState : UnplayableState
{
    public DeadState(Player player)
        : base(player) { }
}

public class GoalState : UnplayableState
{
    public GoalState(Player player)
        : base(player) { }
}
