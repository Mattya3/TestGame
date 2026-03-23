using UnityEngine;
using UnityEngine.InputSystem;
using static GameConstants;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Player : MonoBehaviour
{
    public bool HasReachedGoal { get; private set; } = false;

    public Vector2 InputDirection => _inputDirection;

    [SerializeField, Range(0f, 20f)]
    private float _moveSpeed;

    [SerializeField, Range(0f, 40f)]
    private float _jumpInitialVelocity;

    private Vector2 _inputDirection;
    private Rigidbody2D _rigidBody;
    private Collider2D _collider;

    private const float GROUND_CHECK_THICKNESS = 0.005f; // 接地判定用の定数
    private const float GROUND_MOVE_MARGIN = 0.001f; // 地面との相対速度による接地判定用の定数

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    void Start()
    {
        // Awakeではインスタンスが生成される前に実行される恐れがあるためStart
        GameManager.Instance.RegisterPlayer(this);
        GameManager.Instance.OnPlayerDied += () => enabled = false;
        GameManager.Instance.OnAllPlayersGoal += () => enabled = false;
    }

    void Update()
    {
        _Move();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _inputDirection = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// ジャンプ入力を受け取ります。
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        if (_GetValidGroundHit(Layers.SOLID, Layers.PLAYER).collider == null)
            return;

        float deltaVy = Mathf.Max(
            Mathf.Min(_jumpInitialVelocity, _jumpInitialVelocity - _rigidBody.linearVelocity.y),
            0f
        );

        _rigidBody.AddForce(Vector2.up * deltaVy * _rigidBody.mass, ForceMode2D.Impulse);
    }

    private void _Move()
    {
        Vector2 convertedDirection = GameManager.Instance.ConvertInputDirection(_inputDirection);
        Vector2 groundVelocity = Vector2.zero;

        RaycastHit2D hit = _GetValidGroundHit(Layers.PLAYER);
        Collider2D groundMoveCollider = hit.collider;

        if (groundMoveCollider != null)
        {
            groundVelocity = groundMoveCollider.attachedRigidbody.GetPointVelocity(
                groundMoveCollider.bounds.center
            );
        }

        _rigidBody.linearVelocity = new Vector2(
            convertedDirection.x * _moveSpeed + groundVelocity.x,
            _rigidBody.linearVelocity.y
        );
    }

    /// <summary>
    /// 物理演算を停止します。
    /// </summary>
    public void Freeze()
    {
        _rigidBody.linearVelocity = Vector2.zero;
        _rigidBody.bodyType = RigidbodyType2D.Static;
    }

    /// <summary>
    /// プレイヤーの死亡処理を行います
    /// </summary>
    public void Die(DeathReason deathReason)
    {
        if (!GameManager.Instance.ArePlayersAlive)
            return;

        // TODO: 死亡理由に沿った処理を追加

        GameManager.Instance.HandlePlayerDeath(this, deathReason);
    }

    public void OnGoal()
    {
        if (!GameManager.Instance.ArePlayersAlive)
            return;

        HasReachedGoal = true;
        GameManager.Instance.HandlePlayerGoal(this);
    }

    private RaycastHit2D _GetValidGroundHit(params string[] layerNames)
    {
        RaycastHit2D hit = _GroundCheck(layerNames);
        if (hit.collider == null)
            return hit;

        Rigidbody2D groundRigidbody = hit.collider.attachedRigidbody;
        if (groundRigidbody == null)
            return hit;

        float groundVelocityY = groundRigidbody.GetPointVelocity(_collider.bounds.center).y;
        float relativeVy = groundVelocityY - _rigidBody.linearVelocity.y;

        // 相対上向き速度が閾値（GROUND_MOVE_MARGIN）を超える場合は設定していないものとみなす
        if (relativeVy > GROUND_MOVE_MARGIN)
            return new RaycastHit2D();
        return hit;
    }

    private RaycastHit2D _GroundCheck(params string[] layerNames)
    {
        Bounds bounds = _collider.bounds;
        Vector2 origin = new Vector2(bounds.center.x, bounds.min.y - GROUND_CHECK_THICKNESS * 2);
        Vector2 boxSize = new Vector2(bounds.size.x, GROUND_CHECK_THICKNESS);
        int mask = LayerMask.GetMask(layerNames);
        return Physics2D.BoxCast(origin, boxSize, 0f, Vector2.down, GROUND_CHECK_THICKNESS, mask);
    }
}
