using System;
using UnityEngine;

[Serializable]
public abstract class ExternalEffectBase
{
    [SerializeField]
    private int _priority;

    public int Priority => _priority;
    public virtual Vector2 TransformInput(Player player, Vector2 inputDirection) => inputDirection;
    public virtual float TransformGravityScale(Player player, float gravityScale) => gravityScale;
}
