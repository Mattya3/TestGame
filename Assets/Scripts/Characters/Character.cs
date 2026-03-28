using System;
using UnityEngine;
using static Constants;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class Character : MonoEventReactingBehaviour
{
    [SerializeField, Range(0f, 20f)]
    private float _moveSpeed;

    [SerializeField, Range(0f, 40f)]
    private float _jumpInitialVelocity;

    [SerializeField]
    protected GroundDetector _groundDetector;

    private Collider2D _collider;
    private Rigidbody2D _rigidBody;

    protected virtual void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        _Move();
    }

    protected abstract void _Move();

    protected void _ApplyMovement(Vector2 direction)
    {
        Vector2 groundVelocity = _groundDetector.GetGroundVelocity();
        _rigidBody.linearVelocity = new Vector2(
            direction.x * _moveSpeed + groundVelocity.x,
            _rigidBody.linearVelocity.y
        );
    }

    protected void _ApplyJump()
    {
        if (!_groundDetector.IsGrounded())
            return;

        float deltaVy = Mathf.Max(
            Mathf.Min(_jumpInitialVelocity, _jumpInitialVelocity - _rigidBody.linearVelocity.y),
            0f
        );
        _rigidBody.AddForce(Vector2.up * deltaVy * _rigidBody.mass, ForceMode2D.Impulse);
    }

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
