using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Camera))]
public class CameraFollowController : MonoBehaviour
{
    [SerializeField] private GameObject _target; // カメラが追従する対象
    [SerializeField] private Vector2 _offset; // カメラと対象の位置のオフセット

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        if (_target == null)
        {
            Debug.LogError("カメラにtarget が未設定です", this);
            enabled = false;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var targetPosition = _target.transform.position;
        transform.position = new Vector3(
            targetPosition.x + _offset.x,
            targetPosition.y + _offset.y,
            transform.position.z
        );
    }
}
