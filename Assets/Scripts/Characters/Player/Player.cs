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

    public bool HasReachedGoal { get; private set; } = false;
    public IMoveController MoveController { get; set; }
    public IPlayerState CurrentState => _currentState;
    public IPlayerState PreviousState => _previousState;

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
        _sounds.OnDeath();
        OnDied?.Invoke(deathReason);
    }

    public void Goal()
    {
        _sounds.OnGoal();
        HasReachedGoal = true;

        OnGoal?.Invoke(this);
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

    public void PlayJumpSound()
    {
        _sounds.OnJump();
    }

    public void PlayLandSound()
    {
        _sounds.OnLand();
    }

    private IPlayerState CreateInitialState()
    {
        return _groundDetector.IsGrounded() ? new GroundState(this) : new AirState(this);
    }
}
