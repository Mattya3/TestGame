using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Smoothing")]
    [SerializeField, Min(0f)]
    private float _smoothTimeX = 0.1f;

    [SerializeField, Min(0f)]
    private float _smoothTimeY = 0.1f;

    [Header("Constraints")]
    [SerializeField]
    private CameraBounds _bounds = new CameraBounds(); // カメラの移動制約

    private Camera _camera; // カメラコンポーネントへの参照
    private ICameraTarget _cameraTarget;
    private Vector3 _velocity = Vector3.zero; // カメラの現在の速度

    void Awake()
    {
        _camera = GetComponent<Camera>();
        _cameraTarget = GetComponentInChildren<ICameraTarget>();

        if (!_IsConfigurationValid())
        {
            enabled = false;
            return;
        }
    }

    private bool _IsConfigurationValid()
    {
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
        if (_cameraTarget == null)
        {
            Debug.LogError(
                "CameraControllerにはICameraTargetを実装したコンポーネントが必要です",
                this
            );
            return false;
        }
        return true;
    }

    void Start()
    {
        // カメラの初期位置を設定
        var destination = _CalculateDestination();
        var boundedDestination = _Bound(destination);
        transform.position = boundedDestination;
    }

    void LateUpdate()
    {
        var destination = _CalculateDestination();
        var boundedDestination = _Bound(destination);

        var newPosX = Mathf.SmoothDamp(
            transform.position.x,
            boundedDestination.x,
            ref _velocity.x,
            _smoothTimeX
        );
        var newPosY = Mathf.SmoothDamp(
            transform.position.y,
            boundedDestination.y,
            ref _velocity.y,
            _smoothTimeY
        );
        transform.position = new Vector3(newPosX, newPosY, boundedDestination.z);
    }

    private Vector3 _CalculateDestination()
    {
        return _cameraTarget.Position;
    }

    private Vector3 _Bound(Vector3 pos)
    {
        return _bounds.Bound(pos, transform.position);
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
