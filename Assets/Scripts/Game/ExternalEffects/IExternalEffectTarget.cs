using System;
using UnityEngine;

public interface IExternalEffectTarget
{
    void RegisterInputDirectionConversion(Func<Vector2, Vector2> conversion);
    void SetGravityScale(float gravityScale);
    void SetVerticalMovementStopped(bool stopped);
    float GetDefaultGravityScale();
}
