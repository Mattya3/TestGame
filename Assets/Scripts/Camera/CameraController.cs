using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject _target; // 追従する対象のゲームオブジェクト

    [SerializeField]
    private Vector2 _offset; // 対象からのオフセット

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        if (_target == null)
        {
            Debug.LogError("カメラのtargetが設定されていません", this);
            enabled = false;
        }
    }

    // LateUpdate is called after all Update functions have been called
    void LateUpdate()
    {
        var targetPosition = _target.transform.position;
        transform.position = new Vector3(
            targetPosition.x + _offset.x,
            targetPosition.y + _offset.y,
            transform.position.z
        );
    }
}
