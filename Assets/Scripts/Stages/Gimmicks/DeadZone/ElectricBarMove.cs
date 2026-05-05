using UnityEngine;
using static Constants;

public class ElectricBarMove : MonoEventReactingBehaviour
{
    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private Vector3 _direction = Vector3.right;

    private void Update()
    {
        transform.Translate(_direction * _moveSpeed * Time.deltaTime);
    }

    protected override void OnFailure()
    {
        enabled = false;
    }

    protected override void OnSuccess()
    {
        enabled = false;
    }
}
