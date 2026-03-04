using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    // == Destination ==
    [Header("Destination")]

    [SerializeField]
    private GameObject _target; // 追従する対象のゲームオブジェクト

    [SerializeField]
    private Vector3 _offset; // 対象からのオフセット

    // == Smoothing ==
    [Header("Smoothing")]

    [SerializeField, Min(0f)]
    private float _smoothTime = 0.1f; // カメラの移動のスムーズさを調整するための時間

    private Vector3 _velocity = Vector3.zero; // カメラの現在の速度

    // == Constraints ==
    [Header("Constraints")]

    [SerializeField]
    private bool _lockX = false; // X軸の移動をロックするかどうか

    [SerializeField]
    private bool _lockY = false; // Y軸の移動をロックするかどうか

    [SerializeField]
    private float _minX = float.NegativeInfinity; // カメラのX座標の最小値

    [SerializeField]
    private float _maxX = float.PositiveInfinity; // カメラのX座標の最大値

    [SerializeField]
    private float _minY = float.NegativeInfinity; // カメラのY座標の最小値

    [SerializeField]
    private float _maxY = float.PositiveInfinity; // カメラのY座標の最大値

    // == Component References ==
    private Camera _camera; // カメラコンポーネントへの参照

    // == Unity Event Functions ==

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

        _camera = GetComponent<Camera>();
    }

    // LateUpdate is called after all Update functions have been called
    void LateUpdate()
    {
        var destination = _CalculateDestination();
        transform.position = Vector3.SmoothDamp(
            transform.position,
            destination,
            ref _velocity,
            _smoothTime
        );
    }

    private Vector3 _CalculateDestination()
    {
        var targetPosition = _target.transform.position;
        var destination = targetPosition + _offset;

        if (_lockX) destination.x = transform.position.x;
        else destination.x = Mathf.Clamp(destination.x, _minX, _maxX);
        if (_lockY) destination.y = transform.position.y;
        else destination.y = Mathf.Clamp(destination.y, _minY, _maxY);

        return destination;
    }

    // エディタ上でカメラの移動範囲を視覚化するためのGizmosを描画
    void OnDrawGizmosSelected()
    {
        // デバッグ終了時などに参照が失われる場合があるため、カメラコンポーネントへの参照を再取得
        if (_camera == null)
        {
             _camera = GetComponent<Camera>();
            if (_camera == null)
                return;
        }

        var halfHeight = _camera.orthographicSize;
        var halfWidth = halfHeight * _camera.aspect;
        var cameraSize = new Vector3(
            (_maxX - _minX) + halfWidth * 2,
            (_maxY - _minY) + halfHeight * 2,
            1
        );
        var cameraCenter = new Vector3(
            (_minX + _maxX) / 2,
            (_minY + _maxY) / 2,
            0
        );

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(cameraCenter, cameraSize);
    }
}
