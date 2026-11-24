using UnityEngine;

public interface IExternalEffectTarget
{
    Vector2 ReverseHorizontalInput(Vector2 inputDirection);
    void SetGravityScale(float gravityScale);
    void SetVerticalMovementStopped(bool stopped);
    float GetDefaultGravityScale();
}
