using UnityEngine;

public sealed class PlayerVerticalMovementEffectContext : IVerticalMovementEffectContext
{
    private readonly Rigidbody2D _rigidBody;
    private readonly float _defaultGravityScale;
    private readonly RigidbodyConstraints2D _defaultConstraints;

    public PlayerVerticalMovementEffectContext(Rigidbody2D rigidBody)
    {
        _rigidBody = rigidBody;
        _defaultGravityScale = rigidBody.gravityScale;
        _defaultConstraints = rigidBody.constraints;
    }

    public void SetGravityScale(float gravityScale)
    {
        _rigidBody.gravityScale = gravityScale;
    }

    public float GetDefaultGravityScale()
    {
        return _defaultGravityScale;
    }

    public void SetVerticalMovementStopped(bool stopped)
    {
        if (!stopped)
        {
            _rigidBody.constraints = _defaultConstraints;
            return;
        }

        _rigidBody.constraints |= RigidbodyConstraints2D.FreezePositionY;
        _rigidBody.linearVelocity = new Vector2(_rigidBody.linearVelocity.x, 0f);
    }
}
