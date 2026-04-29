using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Constants;

public class Player : Character
{
    public static event Action<Player> OnCreated;
    public event Action<Player> OnGoal;
    public event Action<DeathReason> OnDied;

    private IPlayerState _currentState;
    private IPlayerState _previousState;
    // private List<IExternalState> _externalStates;

    public IMoveController MoveController { get; set; }
    public IPlayerState CurrentState => _currentState;
    public IPlayerState PreviousState => _previousState;
    public bool IsInGoalState => _currentState is GoalState;

    [SerializeField]
    private PlayerSounds _sounds;

    private Vector2 _inputDirection;

    public Vector2 InputDirection => _inputDirection;

    private void Start()
    {
        if (_sounds == null || !_sounds.IsValid())
        {
            Debug.LogError("PlayerSounds is not properly set up.");
            enabled = false;
            return;
        }

        ChangeState(CreateInitialState());
        OnCreated?.Invoke(this);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _inputDirection = context.ReadValue<Vector2>();
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

    protected override void _Move()
    {
        if (_currentState == null)
            return;

        _currentState.OnMove(_inputDirection);
    }

    public void ChangeState(IPlayerState nextState)
    {
        if (nextState == null)
        {
            Debug.LogError("Next state is null.", this);
            return;
        }

        IPlayerState previousState = _currentState;
        previousState?.OnDisabled();
        _previousState = previousState;
        _currentState = nextState;
        _currentState.OnEnabled();
    }

    public void MoveByInput(Vector2 inputDirection)
    {
        Vector2 convertedDirection = MoveController.ConvertInputDirection(inputDirection);
        _ApplyMovement(convertedDirection);
    }

    public bool IsGrounded()
    {
        return _groundDetector.IsGrounded();
    }

    public bool TryJump()
    {
        if (!IsGrounded())
            return false;

        _ApplyJump();
        return true;
    }

    public void NotifyDied(DeathReason deathReason)
    {
        OnDied?.Invoke(deathReason);
    }

    public void NotifyGoalReached()
    {
        OnGoal?.Invoke(this);
    }

    public void EnterFrozenState()
    {
        if (_currentState == null)
            return;
        if (_currentState is UnplayableState)
            return;

        ChangeState(new FrozenState(this, _sounds));
    }

    private IPlayerState CreateInitialState()
    {
        return _groundDetector.IsGrounded()
            ? new GroundState(this, _sounds)
            : new AirState(this, _sounds);
    }
}
