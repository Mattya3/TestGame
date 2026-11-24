using System;
using UnityEngine;
using static Constants;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AnimationTransitions))]
[RequireComponent(typeof(CharacterEffectsPlayer))]
public abstract class Character : MonoEventReactingBehaviour
{
    public enum FacingDirectionType
    {
        Left = -1,
        Right = 1
    }

    [SerializeField]
    private PlayerActionConfiguration _ActionConfiguration;

    [SerializeField]
    protected GroundDetector _groundDetector;

    private Rigidbody2D _rigidBody;
    private Collider2D _collider;
    private AnimationTransitions _animationTransitions;
    private CharacterEffectsPlayer _effectsPlayer;
    private FacingDirectionType _facingDirection = FacingDirectionType.Right;

    private bool _isInitialized = false;

    protected virtual void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _effectsPlayer = GetComponent<CharacterEffectsPlayer>();
        _animationTransitions = GetComponent<AnimationTransitions>();
    }

    protected virtual void Start()
    {
        EnsureInitialized();
    }

    protected virtual void Initialize() { }

    public void EnsureInitialized()
    {
        if (_isInitialized)
            return;

        Initialize();
        _isInitialized = true;
    }

    protected virtual void Update() {}

    protected virtual void LateUpdate()
    {
        _animationTransitions.Play(GetAnimationName());
    }

    protected virtual void FixedUpdate() { }

    protected virtual string GetAnimationName()
    {
        return AnimationTransitions.IGNORED_ANIMATION_NAME;
    }

    protected FacingDirectionType FacingDirection
    {
        get => _facingDirection;
        set
        {
            _facingDirection = value;
            _animationTransitions.SetFacingDirection(value);
        }
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

    protected override void OnFailure()
    {
    }

    protected override void OnSuccess()
    {
    }

    protected void FreezePhysics()
    {
        _rigidBody.linearVelocity = Vector2.zero;
        _rigidBody.bodyType = RigidbodyType2D.Static;
    }

    protected bool IsGrounded()
    {
        return _groundDetector.IsGrounded();
    }

    protected void SetAnimatorSpeed(float speed)
    {
        _animationTransitions.SetAnimatorSpeed(speed);
    }

    protected void SetAnimatorUpdateMode(AnimatorUpdateMode updateMode)
    {
        _animationTransitions.SetAnimatorUpdateMode(updateMode);
    }


    public Bounds Bounds => _collider.bounds;

    protected CharacterEffectsPlayer EffectsPlayer => _effectsPlayer;

    protected Vector2 CurrentLinearVelocity => _rigidBody.linearVelocity;
}
