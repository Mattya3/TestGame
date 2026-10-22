using UnityEngine;

public sealed class PlayerExternalEffectApplier : IExternalEffectApplier
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

        bool shouldBeActive = _externalEffect.ShouldApply();
        bool shouldActivate = !_isEffectActive && shouldBeActive;
        bool shouldDeactivate = _isEffectActive && !shouldBeActive;

        if (shouldActivate)
        {
            _externalEffect.Apply();
        }
        else if (shouldDeactivate)
        {
            _externalEffect.Reset();
        }

        _isEffectActive = shouldBeActive;
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
