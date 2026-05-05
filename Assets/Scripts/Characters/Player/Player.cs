using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Constants;

public partial class Player : Character
{
    public static event Action<Player> OnCreated;
    public event Action<Player> OnGoal;
    public event Action<DeathReason> OnDied;
    public event Action<Player, Vector2> OnInputDirectionChanged;

    private IPlayerState _currentState;

    [SerializeField]
    private PlayerSounds _sounds;

    private Vector2 _inputDirection;
    private PlayerStateContext _stateContext;

    public bool IsInGoalState => _currentState is GoalState;
    public Vector2 InputDirection => _inputDirection;

    protected override void Awake()
    {
        base.Awake();
        _stateContext = new PlayerStateContext(this);
    }

    private void Start()
    {
        if (_sounds == null || !_sounds.IsValid())
        {
            Debug.LogError("PlayerSounds is not properly set up.");
            enabled = false;
            return;
        }

        _ChangeState(_CreateInitialState());
        OnCreated?.Invoke(this);
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        _inputDirection = context.ReadValue<Vector2>();
        OnInputDirectionChanged?.Invoke(this, _inputDirection);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        if (_currentState == null)
            return;

        _currentState.OnJump();
    }

    public void Die(DeathReason deathReason)
    {
        if (_currentState == null)
            return;

        _currentState.Die(deathReason);
    }

    public void Goal()
    {
        if (_currentState == null)
            return;

        _currentState.Goal();
    }

    public void EnterFrozenState()
    {
        if (_currentState == null)
            return;
        if (_currentState is UnplayableState)
            return;

        _ChangeState(new FrozenState(_stateContext, _sounds));
    }

    public void ApplyExternalEffectType(ExternalEffectType type)
    {
        EffectBehavior behavior = _CreateExternalEffectBehavior(type);
        _stateContext.SetExternalEffectBehavior(behavior);
    }

    public void ResetExternalEffectBehavior()
    {
        _stateContext.ResetExternalEffectBehavior();
    }

    protected override void _Move()
    {
        if (_currentState == null)
            return;

        _currentState.OnMove(_inputDirection);
    }

    private EffectBehavior _CreateExternalEffectBehavior(ExternalEffectType type)
    {
        switch (type)
        {
            case ExternalEffectType.ReverseInput:
                return new ReverseInputBehavior(this);
            case ExternalEffectType.ReverseGravity:
                return new ReverseGravityBehavior(this);
            default:
                return new EffectBehavior(this);
        }
    }

    private void _ChangeState(IPlayerState nextState)
    {
        if (nextState == null)
        {
            Debug.LogError("Next state is null.", this);
            return;
        }

        _currentState?.OnDisabled();
        _currentState = nextState;
        _currentState.OnEnabled();
    }

    private void _MoveByInput(Vector2 inputDirection)
    {
        _ApplyMovement(inputDirection);
    }

    private bool _IsGrounded()
    {
        return _groundDetector.IsGrounded();
    }

    private bool _TryJump()
    {
        if (!_IsGrounded())
            return false;

        _ApplyJump();
        return true;
    }

    private void _NotifyDied(DeathReason deathReason)
    {
        OnDied?.Invoke(deathReason);
    }

    private void _NotifyGoalReached()
    {
        OnGoal?.Invoke(this);
    }

    private IPlayerState _CreateInitialState()
    {
        return _groundDetector.IsGrounded()
            ? new GroundState(_stateContext, _sounds)
            : new AirState(_stateContext, _sounds);
    }
}
