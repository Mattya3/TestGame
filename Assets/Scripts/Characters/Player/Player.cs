using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Constants;

[RequireComponent(typeof(PlayerInputSetup))]
[RequireComponent(typeof(StageSceneContextReadonlyAccess))]
public partial class Player : Character
{
    public event Action<Player> OnGoal;
    public event Action<DeathReason> OnDied;

    private IPlayerState _currentState;

    private PlayerExternalEffectApplier _externalEffectApplier;
    private Vector2 _inputDirection;
    private IPlayerStateContext _stateContext;

    private AnimationTransitions _animationTransitions;
    private StageSceneContextReadonlyAccess _stageContextAccess;

    public bool IsInGoalState => _currentState?.HasReachedGoal ?? false;
    public bool IsAlive => _currentState?.IsAlive ?? true;
    public Vector2 InputDirection => _inputDirection;
    public IExternalEffectApplier ExternalEffectApplier => _externalEffectApplier;

    protected override void Awake()
    {
        base.Awake();

        _stageContextAccess = GetComponent<StageSceneContextReadonlyAccess>();

        _stateContext = new StateContext(this);
        _externalEffectApplier = new PlayerExternalEffectApplier();

        _animationTransitions = GetComponentInChildren<AnimationTransitions>();
        if (_animationTransitions == null)
        {
            Debug.LogError("AnimationTransitions component not found in Player's children.");
            enabled = false;
            return;
        }
    }

    protected override void Start()
    {
        // Character側でInitializeが呼ばれるので，ここでは呼ばない
        base.Start();
    }

    protected override void Initialize()
    {
        _ChangeState(_CreateInitialState());
    }

    protected override void Update()
    {
        base.Update();

        // memo: このメソッドはここではない気がするが(character側にこれを置きたい), 次issueで対応
        _externalEffectApplier.UpdateEffectState();

        if (_currentState == null)
            return;

        _currentState.Update(_inputDirection);
        _animationTransitions.SetFacingDirection(FacingDirection);
        _animationTransitions.Play(_currentState.GetAnimationName());
    }

    protected override void OnGamePlayStart()
    {
        base.OnGamePlayStart();
        _ChangeState(_CreateGamePlayStartState());
    }

    public void SetupPlayerInput(int playerIndex)
    {
        var inputSetup = GetComponent<PlayerInputSetup>();
        inputSetup.Setup(playerIndex);
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        _inputDirection = context.ReadValue<Vector2>();
    }

    public void OnJumpInput(InputAction.CallbackContext context)
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

        _ChangeState(new FrozenState(_stateContext));
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
        Vector2 direction = _externalEffectApplier.GetMoveDirection(inputDirection);
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

    private void _SetAnimatorSpeed(float speed)
    {
        _animationTransitions.SetAnimatorSpeed(speed);
    }

    private void _SetAnimatorUpdateMode(AnimatorUpdateMode updateMode)
    {
        _animationTransitions.SetAnimatorUpdateMode(updateMode);
    }

    private void _PlayCharacterEffect(string effectKey)
    {
        EffectsPlayer.Play(effectKey, FacingDirection);
    }

    private void _StopCharacterEffect(string effectKey)
    {
        EffectsPlayer.Stop(effectKey);
    }

    private IPlayerState _CreateInitialState()
    {
        // リスタート時はEntranceStateを経由せずにゲームプレイ開始状態に遷移する
        if (_stageContextAccess.AfterRestart)
            return _CreateGamePlayStartState();

        return new EntranceState(_stateContext);
    }

    private IPlayerState _CreateGamePlayStartState()
    {
        return _groundDetector.IsGrounded()
            ? new GroundState(_stateContext)
            : new AirState(_stateContext);
    }
}
