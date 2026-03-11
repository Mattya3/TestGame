using UnityEngine;

public class ElecticBarMove : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;

    void Start()
    {
        GameManager.Instance.OnPlayerDied += () => enabled = false;
    }

    void Update()
    {
        transform.Translate(Vector3.right * _moveSpeed * Time.deltaTime);
    }
}
