using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Player : MonoBehaviour
{
    [SerializeField, Range(0f, 20f)]
    private float _moveSpeed;

    [SerializeField, Range(0f, 40f)]
    private float _jumpForce;

    [SerializeField]
    private float _deathYThreshold;

    private Vector2 _inputDirection;
    private Rigidbody2D _rigidBody; // _rb と略すこともあるが，初回なのでわかりやすく
    private Collider2D _collider;

    private bool _isDead = false;

    private const float GROUND_CHECK_THICKNESS = 0.005f; // 接地判定用の定数
    private const string SOLID_LAYER_NAME = "Solid"; // 上に乗れるオブジェクトのレイヤ０
    private const string DEAD_ZONE_TAG = "DeadZone"; // リスタート発生判定用のタグ

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (_NeedRestart())
        {
            _RestartStage();
            return;
        }
        _Move();
    }

    /// <summary>
    /// 移動入力を受け取ります。
    /// </summary>
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(DEAD_ZONE_TAG))
        {
            _isDead = true;
        }
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
            LayerMask.GetMask(SOLID_LAYER_NAME)
        );

        return hit.collider != null;
    }

    private bool _IsFallen() => transform.position.y < _deathYThreshold;

    private bool _NeedRestart() => _IsFallen() || _isDead;

    private void _RestartStage() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
