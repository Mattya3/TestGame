using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Constants;

public class Player : Character
{
    public event Action<Player> OnGoal;
    public event Action<DeathReason> OnDied;

    public bool HasReachedGoal { get; private set; } = false;
    public IMoveController MoveController { get; set; }

    [SerializeField]
    private PlayerSounds _sounds;

    private Vector2 _inputDirection;

    // 後でPlayersManagerがPlayerをInstantiateするようになったら、ここでPlayersCollectionAccessを取得するのはやめる (RegisterPlayerも要らなくなる)
    private PlayersCollectionAccess _playersCollection;

    public Vector2 InputDirection => _inputDirection;

    protected override void Awake()
    {
        base.Awake();

        if (_sounds == null || !_sounds.IsValid())
        {
            Debug.LogError("PlayerSounds is not properly set up.");
            enabled = false;
        }

        _playersCollection = GetComponent<PlayersCollectionAccess>();
    }

    private void Start()
    {
        _playersCollection?.RegisterPlayer(this);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _inputDirection = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        _ApplyJump();
        _sounds.OnJump();
    }

    public void Die(DeathReason deathReason)
    {
        _sounds.OnDeath();
        OnDied?.Invoke(deathReason);
    }

    public void Goal()
    {
        _sounds.OnGoal();
        HasReachedGoal = true;

        OnGoal?.Invoke(this);
    }

    protected override void _Move()
    {
        Vector2 convertedDirection = MoveController.ConvertInputDirection(_inputDirection);
        _ApplyMovement(convertedDirection);
    }

    // TODO: ここではない気がするが，GroundDetectorは変更されると思うのでこのまま
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer(Layers.SOLID))
            return;
        if (!_groundDetector.IsGrounded())
            return; // 接触時に着地しているかで判定

        _sounds.OnLand();
    }
}
