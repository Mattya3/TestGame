using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Constants;

public partial class Player : Character
{
    public static event Action<Player> OnCreated;
    public event Action<Player> OnGoal;
    public event Action<DeathReason> OnDied;

    private IPlayerState _currentState;

    // private List<IExternalState> _externalStates;

    [SerializeField]
    private PlayerSounds _sounds;

    private PlayerExternalEffectContext _externalEffectContext;
    private Vector2 _inputDirection;
    private IPlayerStateContext _stateContext;

    public bool IsInGoalState => _currentState is GoalState;
    public Vector2 InputDirection => _inputDirection;
    public IExternalEffectContext ExternalEffectContext => _externalEffectContext;

    private void Start()
    {
        if (_sounds == null || !_sounds.IsValid())
        {
            Debug.LogError("PlayerSounds is not properly set up.");
            enabled = false;
            return;
        }

        _stateContext = new StateContext(this);
        _externalEffectContext = new PlayerExternalEffectContext(this);
        _ChangeState(_CreateInitialState());
        OnCreated?.Invoke(this);
    }

    protected override void _Move()
    {
        // memo: このメソッドはここではない気がするが(character側にこれを置きたい), 次issueで対応
        _externalEffectContext.UpdateEffectState();

        if (_currentState == null)
            return;

        _currentState.OnMove(_inputDirection);
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

    public void EnterFrozenState()
    {
        if (_currentState == null)
            return;
        if (_currentState is UnplayableState)
            return;

        _ChangeState(new FrozenState(_stateContext, _sounds));
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
        Vector2 direction = _externalEffectContext.GetMoveDirection(inputDirection);
        _ApplyMovement(direction);
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

    private void _SetGravityScaleForExternalEffect(float gravityScale)
    {
        _SetGravityScale(gravityScale);
    }

    private float _GetDefaultGravityScaleForExternalEffect()
    {
        return _GetDefaultGravityScale();
    }
}
