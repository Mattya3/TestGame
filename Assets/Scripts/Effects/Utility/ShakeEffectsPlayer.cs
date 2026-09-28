using System.Collections.Generic;
using UnityEngine;

public class ShakeEffectsPlayer : MonoBehaviour
{
    private interface IActiveShake
    {
        void UpdateTime();
        bool IsExpired();
        Vector2 CalculateOffset();
        bool IsContinuous();
    }

    private abstract class ActiveShakeBase : IActiveShake
    {
        private readonly ShakeEffect _effect;
        private readonly float _phaseX;
        private readonly float _phaseY;
        private float _elapsedTime;

        protected ActiveShakeBase(ShakeEffect effect)
        {
            _effect = effect;
            _elapsedTime = 0f;
            _phaseX = effect.PhaseOffsets.x;
            _phaseY = effect.PhaseOffsets.y;
        }

        public void UpdateTime()
        {
            float deltaTime =
                _effect.UpdateMode == ShakeEffect.ShakeUpdateMode.UnscaledTime
                    ? Time.unscaledDeltaTime
                    : Time.deltaTime;

            _elapsedTime += deltaTime;
        }

        public abstract bool IsExpired();

        public abstract Vector2 CalculateOffset();

        public abstract bool IsContinuous();

        protected float CalculateProgress()
        {
            return _elapsedTime / _effect.Duration;
        }

        protected Vector2 CalculateOffsetWithAmplitude(float amplitude)
        {
            float offsetX =
                Mathf.Sin(_elapsedTime * _effect.Frequency.x * Mathf.PI * 2f + _phaseX)
                * _effect.Magnitude.x
                * amplitude;
            float offsetY =
                Mathf.Sin(_elapsedTime * _effect.Frequency.y * Mathf.PI * 2f + _phaseY)
                * _effect.Magnitude.y
                * amplitude;

            return new Vector2(offsetX, offsetY);
        }
    }

    private class OneShotShake : ActiveShakeBase
    {
        public OneShotShake(ShakeEffect effect)
            : base(effect) { }

        public override bool IsExpired()
        {
            float progress = CalculateProgress();
            return progress >= 1.0f;
        }

        public override Vector2 CalculateOffset()
        {
            float progress = CalculateProgress();
            float amplitude = 1.0f - progress;
            return CalculateOffsetWithAmplitude(amplitude);
        }

        public override bool IsContinuous()
        {
            return false;
        }
    }

    private class ContinuousShake : ActiveShakeBase
    {
        public ContinuousShake(ShakeEffect effect)
            : base(effect) { }

        public override bool IsExpired()
        {
            return false;
        }

        public override Vector2 CalculateOffset()
        {
            return CalculateOffsetWithAmplitude(1.0f);
        }

        public override bool IsContinuous()
        {
            return true;
        }
    }

    private readonly List<IActiveShake> _activeShakes = new List<IActiveShake>();
    private Vector2 _currentShakeOffset = Vector2.zero;

    public void Play(ShakeEffect shakeEffect)
    {
        if (shakeEffect == null)
        {
            Debug.LogWarning("ShakeEffect is null.");
            return;
        }

        IActiveShake shake =
            shakeEffect.Type == ShakeEffect.ShakeType.OneShot
                ? (IActiveShake)new OneShotShake(shakeEffect)
                : new ContinuousShake(shakeEffect);

        _activeShakes.Add(shake);
    }

    public Vector2 CurrentShakeOffset => _currentShakeOffset;

    private void Update()
    {
        Vector2 combinedOffset = Vector2.zero;

        for (int i = _activeShakes.Count - 1; i >= 0; i--)
        {
            IActiveShake shake = _activeShakes[i];

            shake.UpdateTime();

            if (shake.IsExpired())
            {
                _activeShakes.RemoveAt(i);
                continue;
            }

            Vector2 offset = shake.CalculateOffset();
            combinedOffset += offset;
        }

        _currentShakeOffset = combinedOffset;
    }

    public void StopAll()
    {
        _activeShakes.Clear();
        _currentShakeOffset = Vector2.zero;
    }

    public void StopContinuous()
    {
        for (int i = _activeShakes.Count - 1; i >= 0; i--)
        {
            if (_activeShakes[i].IsContinuous())
            {
                _activeShakes.RemoveAt(i);
            }
        }
    }
}
