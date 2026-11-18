using UnityEngine;
using static Constants;

public class EntranceState : UnplayableState
{
    private static readonly int NUM_ENTRANCE_ANIMATIONS = 6;

    private int _entranceAnimationIndex = 0;

    public EntranceState(IPlayerStateContext context)
        : base(context)
    {
        _entranceAnimationIndex = Random.Range(0, NUM_ENTRANCE_ANIMATIONS);
    }

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
        return $"Entrance{_entranceAnimationIndex + 1}";
    }
}
