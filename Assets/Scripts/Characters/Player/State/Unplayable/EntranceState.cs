using UnityEngine;
using static Constants;

public class EntranceState : UnplayableState
{
    public EntranceState(IPlayerStateContext context)
        : base(context) { }

    public override void OnEnabled()
    {
    }

    public override void Die(DeathReason deathReason)
    {
        Debug.LogWarning("Die() was called while in EntranceState.", Context.LogContext);
    }

    public override void Goal()
    {
        Debug.LogWarning("Goal() was called while in EntranceState.", Context.LogContext);
    }

    public override string GetAnimationName()
    {
        return "Entrance4";
    }
}
