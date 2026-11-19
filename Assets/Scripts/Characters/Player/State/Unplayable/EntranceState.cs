using UnityEngine;
using static Constants;

public class EntranceState : UnplayableState
{
    private static readonly int NUM_ENTRANCE_ANIMATIONS = 6;
    private static readonly float ENTRANCE_ANIMATION_MAX_DELAY = 0.05f;

    private int _entranceAnimationIndex = 0;
    private float _entranceAnimationDelay = 0.0f;

    public EntranceState(IPlayerStateContext context)
        : base(context)
    {
        _entranceAnimationIndex = Random.Range(0, NUM_ENTRANCE_ANIMATIONS);
    }

    public override void OnEnabled()
    {
        base.OnEnabled();
        _entranceAnimationDelay = Random.Range(0.0f, ENTRANCE_ANIMATION_MAX_DELAY);
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
        // ランダムに再生を遅延
        if (ElapsedTimeInState < _entranceAnimationDelay)
            return AnimationTransitions.IGNORED_ANIMATION_NAME;

        return $"Entrance{_entranceAnimationIndex + 1}";
    }
}
