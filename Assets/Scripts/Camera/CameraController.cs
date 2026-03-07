using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject _target; // 追従する対象のゲームオブジェクト

    [SerializeField]
    private Vector3 _offset; // 対象からのオフセット

    [SerializeField, Min(0f)]
    private float _smoothTime = 0.1f; // カメラの移動のスムーズさを調整するための時間

    private Vector3 _velocity = Vector3.zero; // カメラの現在の速度

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        if (_target == null)
        {
            Debug.LogError("カメラのtargetが設定されていません", this);
            enabled = false;
        }
        if (_offset.z >= 0)
        {
            Debug.LogError("カメラのoffset.zは負の値でなければなりません", this);
            enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
         // カメラの初期位置を設定
        transform.position = _target.transform.position + _offset;
    }

    // LateUpdate is called after all Update functions have been called
    void LateUpdate()
    {
        var targetPosition = _target.transform.position;
        var destination = targetPosition + _offset;
        transform.position = Vector3.SmoothDamp(
            transform.position,
            destination,
            ref _velocity,
            _smoothTime
        );
    }
}
