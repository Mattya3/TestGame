using UnityEngine;
using static GameConstants;

public class ElectricBarMove : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private Vector3 _direction = Vector3.right;

    private void Update()
    {
        transform.Translate(_direction * _moveSpeed * Time.deltaTime);
    }
}
