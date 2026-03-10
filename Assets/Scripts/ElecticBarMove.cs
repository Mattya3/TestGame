using UnityEngine;

public class ElecticBarMove : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;

    void Update()
    {
        if (!GameManager.Instance.IsPlayerAlive)
            return;

        transform.Translate(Vector3.right * _moveSpeed * Time.deltaTime);
    }
}
