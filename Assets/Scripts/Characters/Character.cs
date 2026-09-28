using CharacterState;
using UnityEngine;
using static Constants;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract partial class Character : MonoEventReactingBehaviour
{
    [SerializeField]
    private PlayerActionConfiguration _ActionConfiguration;

    [SerializeField]
    protected GroundDetector _groundDetector;

    private Rigidbody2D _rigidBody;
    private Collider2D _collider;
    private ICharacterState _characterState;
    private ICharacterStateContext _characterStateContext;

    protected ICharacterState _CurrentState => _characterState;

    protected ICharacterStateContext _StateContext => _characterStateContext;

    protected abstract ICharacterSounds _StateSounds { get; }

    protected virtual void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _characterStateContext = new CharacterStateContext(this);
    }

    protected virtual void Start()
    {
        _ChangeState(_CreateInitialState());
    }

    protected virtual void Update()
    {
        _Move();
    }

    protected virtual void _Move()
    {
        _BeforeStateMove();
        _characterState?.OnMove();
    }

    protected virtual void _BeforeStateMove() { }

    protected virtual void _MoveCharacter() { }

    protected virtual ICharacterState _CreateInitialState()
    {
        return _groundDetector.IsGrounded()
            ? new CharacterGroundState(_characterStateContext, _StateSounds)
            : new CharacterAirState(_characterStateContext, _StateSounds);
    }

    protected void _ChangeState(ICharacterState nextState)
    {
        if (nextState == null)
        {
            Debug.LogError("Next character state is null.", this);
            return;
        }

        _characterState?.OnDisabled();
        _characterState = nextState;
        _characterState.OnEnabled();
    }

    protected void _ApplyMovement(Vector2 direction)
    {
        Vector2 groundVelocity = _groundDetector.GetGroundVelocity();
        _rigidBody.linearVelocity = new Vector2(
            direction.x * _ActionConfiguration._moveSpeed + groundVelocity.x,
            _rigidBody.linearVelocity.y
        );
    }

    protected void _ApplyJump()
    {
        if (!_groundDetector.IsGrounded())
            return;

        float deltaVy = Mathf.Max(
            _ActionConfiguration._jumpInitialVelocity - _rigidBody.linearVelocity.y,
            0f
        );
        _rigidBody.AddForce(Vector2.up * deltaVy * _rigidBody.mass, ForceMode2D.Impulse);
    }

    public virtual void Die(DeathReason deathReason)
    {
        _characterState?.Die(deathReason);
    }

    public void EnterFrozenState()
    {
        if (_CurrentState == null)
            return;
        if (_CurrentState is CharacterUnplayableState)
            return;

        _ChangeState(new CharacterFrozenState(_StateContext, _StateSounds));
    }

    protected virtual void _NotifyDied(DeathReason deathReason) { }

    protected virtual void _NotifyGoalReached() { }

    protected override void OnFailure()
    {
        enabled = false;
    }

    protected override void OnSuccess()
    {
        enabled = false;
    }

    public void Freeze()
    {
        _rigidBody.linearVelocity = Vector2.zero;
        _rigidBody.bodyType = RigidbodyType2D.Static;
    }

    public Bounds Bounds => _collider.bounds;
}
