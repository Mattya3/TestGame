using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    // == Destination ==
    [Header("Destination")]
    [SerializeField]
    private Constants.CameraTargetMode _targetMode = Constants.CameraTargetMode.Players;

    private ICameraTarget _cameraTarget;

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
    private CameraBounds _bounds = new CameraBounds(); // カメラの移動制約

    // == Component References ==
    private Camera _camera; // カメラコンポーネントへの参照

    // == Unity Event Functions ==

    void Awake()
    {
        if (!_IsConfigurationValid())
        {
            enabled = false;
            return;
        }
        _camera = GetComponent<Camera>();
    }

    private bool _IsConfigurationValid()
    {
        if (_offset.z >= 0)
        {
            Debug.LogError("カメラのoffset.zは負の値でなければなりません", this);
            return false;
        }
        if (_bounds.HasReversedBounds())
        {
            Debug.LogError("カメラ制約の最小値は最大値より小さくなければなりません", this);
            return false;
        }
        if (_bounds.HasNaN())
        {
            Debug.LogError("カメラの制約に無効な値が含まれています", this);
            return false;
        }
        return true;
    }

    void Start()
    {
        try
        {
            _cameraTarget = CameraTargetFactory.Create(_targetMode);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"カメターゲットの作成に失敗: {ex.Message}", this);
            enabled = false;
            return;
        }

        // カメラの初期位置を設定
        var destination = _CalculateDestination();
        transform.position = destination;
    }

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
        var targetPosition = _cameraTarget.Position();
        return _bounds.Bound(targetPosition + _offset, transform.position);
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

        _bounds.DrawGizmos(_camera, transform.position);
    }
}
