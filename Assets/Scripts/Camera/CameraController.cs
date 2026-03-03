using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject _target; // 追従する対象のゲームオブジェクト

    [SerializeField]
    private Vector3 _offset; // 対象からのオフセット

    private float _originalZ = 0f; // カメラの元のZ位置。デフォルトで0でないので、Awakeで保存しておく

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        if (_target == null)
        {
            Debug.LogError("カメラのtargetが設定されていません", this);
            enabled = false;
        }

        _originalZ = transform.position.z; // カメラの元のZ位置を保存
    }

    // LateUpdate is called after all Update functions have been called
    void LateUpdate()
    {
        var targetPosition = _target.transform.position;
        transform.position = new Vector3(
            targetPosition.x + _offset.x,
            targetPosition.y + _offset.y,
            _originalZ + _offset.z
        );
    }
}
