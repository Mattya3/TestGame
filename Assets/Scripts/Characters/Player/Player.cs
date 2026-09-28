using System;
using CharacterState;
using UnityEngine;
using UnityEngine.InputSystem;
using static Constants;

public partial class Player : Character
{
    public event Action<Player> OnGoal;
    public event Action<DeathReason> OnDied;

    [SerializeField]
    private PlayerSounds _sounds;

    private PlayerExternalEffectApplier _externalEffectApplier;
    private Vector2 _inputDirection;

    public bool IsInGoalState => _CurrentState is GoalState;
    public Vector2 InputDirection => _inputDirection;
    public IExternalEffectApplier ExternalEffectApplier => _externalEffectApplier;

    protected override ICharacterSounds _StateSounds => _sounds;

    protected override void Awake()
    {
        base.Awake();

        if (_sounds == null || !_sounds.IsValid())
        {
            Debug.LogError("PlayerSounds is not properly set up.");
            enabled = false;
            return;
        }

        _externalEffectApplier = new PlayerExternalEffectApplier();
    }

    protected override void _BeforeStateMove()
    {
        // memo: このメソッドはここではない気がするが(character側にこれを置きたい)
        // memo: 敵キャラを正式実装してから対処したい
        _externalEffectApplier.UpdateEffectState();
    }

    protected override void _MoveCharacter()
    {
        Vector2 direction = _externalEffectApplier.GetMoveDirection(_inputDirection);
        _ApplyMovement(direction);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _inputDirection = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        _CurrentState?.OnJump();
    }

    public void Goal()
    {
        if (_CurrentState == null)
            return;
        if (_CurrentState is CharacterUnplayableState)
            return;

        _ChangeState(new GoalState(_StateContext, _sounds));
    }

    protected override void _NotifyDied(DeathReason deathReason)
    {
        OnDied?.Invoke(deathReason);
    }

    protected override void _NotifyGoalReached()
    {
        OnGoal?.Invoke(this);
    }
}
