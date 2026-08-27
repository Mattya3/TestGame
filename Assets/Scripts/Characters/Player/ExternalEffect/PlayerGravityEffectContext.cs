using UnityEngine;

public sealed class PlayerGravityEffectContext : IGravityEffectContext
{
    private readonly Rigidbody2D _rigidBody;
    private readonly float _defaultGravityScale;

    public PlayerGravityEffectContext(Rigidbody2D rigidBody)
    {
        _rigidBody = rigidBody;
        _defaultGravityScale = rigidBody.gravityScale;
    }

    public void SetGravityScale(float gravityScale)
    {
        _rigidBody.gravityScale = gravityScale;
    }

    public float GetDefaultGravityScale()
    {
        return _defaultGravityScale;
    }
}
