using System;
using UnityEngine;

public partial class Player
{
    public sealed class PlayerExternalEffectTarget : IExternalEffectTarget
    {
        private Player _player;
        private Rigidbody2D _rigidbody;
        private float _defaultGravityScale = 1.0f;
        private RigidbodyConstraints2D _originalConstraints;

        public PlayerExternalEffectTarget(Player player)
        {
            _player = player;
            _rigidbody = player.GetComponent<Rigidbody2D>();
            _defaultGravityScale = _rigidbody.gravityScale;
            _originalConstraints = _rigidbody.constraints;
        }

        public void RegisterInputDirectionConversion(Func<Vector2, Vector2> conversion)
        {
            _player._inputDirectionConversion = conversion;
        }

        public void SetGravityScale(float gravityScale)
        {
            _rigidbody.gravityScale = gravityScale;
        }

        public void SetVerticalMovementStopped(bool stopped)
        {
            if (!stopped)
            {
                _rigidbody.constraints = _originalConstraints;
                return;
            }

            _rigidbody.constraints |= RigidbodyConstraints2D.FreezePositionY;
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, 0f);
        }

        public float GetDefaultGravityScale()
        {
            return _defaultGravityScale;
        }
    }
    
}
