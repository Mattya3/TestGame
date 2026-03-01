using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollowController : MonoBehaviour
{
    [SerializeField] private GameObject _target; // カメラが追従する対象
    [SerializeField] private Vector2 _offset; // カメラと対象の位置のオフセット

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_target == null)
            return;

        var targetPosition = _target.transform.position;
        transform.position = new Vector3(
            targetPosition.x + _offset.x,
            targetPosition.y + _offset.y,
            transform.position.z
        );
    }
}
