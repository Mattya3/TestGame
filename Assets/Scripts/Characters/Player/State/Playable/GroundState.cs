using UnityEngine;

public class GroundState : PlayableState
{
    private static readonly string _EFFECT_KEY_JUMP = "Jump";

    private bool _jumpRequested = false;

    public GroundState(IPlayerStateContext context)
        : base(context) { }

    public override void Update(Vector2 inputDirection)
    {
        base.Update(inputDirection);
    }

    public override void FixedUpdate(Vector2 inputDirection)
    {
        base.FixedUpdate(inputDirection);

        Context.MoveByInput(inputDirection);
        Context.SetFacingDirectionByVector(inputDirection);

        if (!Context.IsGrounded())
        {
            Context.ChangeState(new AirState(Context));
            return;
        }
        if (_jumpRequested)
        {
            _jumpRequested = false;
            _TryJump();
            return;
        }
    }

    public override void OnJumpInput()
    {
        _jumpRequested = true;
    }

    public override string GetAnimationName()
    {
        return Mathf.Approximately(Context.CurrentInputDirection.x, 0) ? "Idle" : "Run";
    }

    private void _TryJump()
    {
        if (!Context.TryJump())
            return;
        
        Context.PlayCharacterEffect(_EFFECT_KEY_JUMP);
        Context.ChangeState(new AirState(Context));
    }
}
