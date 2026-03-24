using UnityEngine;
using static GameConstants;

public class ElectricBarMove : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;

    void Start()
    {
        GameManager.Instance.RegisterEventAction(GameEvent.Failure, () => enabled = false);
        GameManager.Instance.RegisterEventAction(GameEvent.Success, () => enabled = false);
    }

    void Update()
    {
        transform.Translate(Vector3.right * _moveSpeed * Time.deltaTime);
    }
}
