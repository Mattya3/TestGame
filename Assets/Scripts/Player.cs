using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField, Range(0f, 20f)]
    private float _moveSpeed;
    [SerializeField, Range(0f, 40f)]
    private float _jumpForce;

    private Vector2 _inputDirection;
    private Rigidbody2D _rigidBody; // _rb と略すこともあるが，初回なのでわかりやすく

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
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
        if (context.performed && _GroundCheck())
        {
            _rigidBody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    private void _Move()
    {
        _rigidBody.linearVelocity = new Vector2(
            _inputDirection.x * _moveSpeed,
            _rigidBody.linearVelocity.y
        );
    }

    private bool _GroundCheck()
    {
        // キャラクターの足元に接触判定を飛ばして地面に接触しているか判定

        // 開始地点をプレイヤーの中心から 0.65f（足元より少し下）にずらし，自身を判定範囲から除外
        Vector2 origin = (Vector2)transform.position + Vector2.down * 0.65f;
        // ごく短い距離（0.005f）だけ下に判定を飛ばす
        Vector2 boxSize = new Vector2(1f, 0.005f);
        // ~0 はすべてのレイヤーを意味する(必要に応じて修正が必要). 何かしらが下に存在する場合trueとなる
        RaycastHit2D hit = Physics2D.BoxCast(origin, boxSize, 0f, Vector2.down, 0.005f, ~0);
        return hit.collider != null;
    }
}
