using UnityEngine;
using UnityEngine.InputSystem;
using static GameConstants;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Player : MonoBehaviour
{
    [SerializeField, Range(0f, 20f)]
    private float _moveSpeed;

    [SerializeField, Range(0f, 40f)]
    private float _jumpForce;

    private Vector2 _inputDirection;
    private Rigidbody2D _rigidBody;
    private Collider2D _collider;

    private const float GROUND_CHECK_THICKNESS = 0.005f; // 接地判定用の定数

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
        if (!_IsGrounded())
            return;

        _rigidBody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    private void _Move()
    {
        _rigidBody.linearVelocity = new Vector2(
            _inputDirection.x * _moveSpeed,
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
        if (!GameManager.Instance.IsPlayerAlive)
            return;

        // TODO: 死亡理由に沿った処理を追加

        GameManager.Instance.HandlePlayerDeath(this, deathReason);
    }

    private bool _IsGrounded()
    {
        // コライダの境界情報（Bounds）を使用して，足元の位置と幅を動的に計算
        Bounds bounds = _collider.bounds;

        // 開始地点：コライダの下端中央
        Vector2 origin = new Vector2(bounds.center.x, bounds.min.y - GROUND_CHECK_THICKNESS * 2);

        // ボックスのサイズ：コライダの横幅と同じ，高さはごく薄く設定
        Vector2 boxSize = new Vector2(bounds.size.x, GROUND_CHECK_THICKNESS);

        // 下方向にわずかにキャストして接触を確認
        RaycastHit2D hit = Physics2D.BoxCast(
            origin,
            boxSize,
            0f,
            Vector2.down,
            GROUND_CHECK_THICKNESS,
            LayerMask.GetMask(Layers.SOLID)
        );

        return hit.collider != null;
    }
}
