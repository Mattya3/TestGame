using System;
using UnityEngine;
using static Constants;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class Character : MonoEventReactingBehaviour
{
    [SerializeField]
    private PlayerActionConfiguration _ActionConfiguration;

    [SerializeField]
    protected GroundDetector _groundDetector;

    private Rigidbody2D _rigidBody;
    private Collider2D _collider;

    protected virtual void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
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
