using UnityEngine;

public class ElecticBarMove : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5f;

    void Update()
    {
        transform.Translate(Vector3.right * _moveSpeed * Time.deltaTime);
    }
}
