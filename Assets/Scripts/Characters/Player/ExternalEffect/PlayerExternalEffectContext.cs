using UnityEngine;

public sealed class PlayerExternalEffectContext : IExternalEffectContext
{
    private IExternalEffect _externalEffect;
    private bool _isEffectActive;

    public void UpdateEffectState()
    {
        if (_externalEffect == null)
        {
            _isEffectActive = false;
            return;
        }

        bool shouldApply = _externalEffect.ShouldApply();
        if (!_isEffectActive && shouldApply)
        {
            _externalEffect.Apply();
        }
        else if (_isEffectActive && !shouldApply)
        {
            _externalEffect.Reset();
        }

        _isEffectActive = shouldApply;
    }

    public Vector2 GetMoveDirection(Vector2 inputDirection)
    {
        if (!_isEffectActive)
            return inputDirection;

        if (_externalEffect is IInputDirectionEffect inputDirectionEffect)
            return inputDirectionEffect.ConvertInputDirection(inputDirection);

        return inputDirection;
    }

    public void SetExternalEffect(IExternalEffect externalEffect)
    {
        _externalEffect?.Reset();
        _externalEffect = externalEffect;
        _isEffectActive = false;
    }
}
