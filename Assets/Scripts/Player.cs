using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour{
    [SerializeField] private float _moveSpeed;

    private Vector2 _inputDirection;
    private Rigidbody2D _rigidBody;  // _rb と略すこともあるが，初回なのでわかりやすく

    void Start(){
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update(){
        _Move();
    }

    private void _Move(){
        _rigidBody.linearVelocity = new Vector2(_inputDirection.x * _moveSpeed, _rigidBody.linearVelocity.y);
    }

    public void OnMove(InputAction.CallbackContext context){
        _inputDirection = context.ReadValue<Vector2>();
    }
}

