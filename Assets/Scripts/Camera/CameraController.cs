using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private GameObject _colliderRoot;

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
        _camera = GetComponentInChildren<Camera>();
        _cameraTarget = GetComponentInChildren<ICameraTarget>();

        if (!_IsConfigurationValid())
        {
            enabled = false;
            return;
        }
    }

    private bool _IsConfigurationValid()
    {
        if (_colliderRoot == null)
        {
            Debug.LogError("CameraControllerにはColliderオブジェクトが必要です", this);
            return false;
        }
        if (_camera == null)
        {
            Debug.LogError("CameraControllerにはCameraコンポーネントが必要です", this);
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
        _cameraTarget.OnStart();

        // カメラの初期位置を設定
        var destination = _CalculateDestination();
        var boundedDestination = _Bound(destination);

        _colliderRoot.transform.position = boundedDestination;
        _camera.transform.position = boundedDestination;
    }

    private void FixedUpdate()
    {
        var destination = _CalculateDestination();
        var boundedDestination = _Bound(destination);

        _colliderRoot.transform.position = new Vector3(
            boundedDestination.x,
            boundedDestination.y,
            boundedDestination.z
        );
    }

    void LateUpdate()
    {
        var destination = _colliderRoot.transform.position;

        var newPosX = Mathf.SmoothDamp(
            _camera.transform.position.x,
            destination.x,
            ref _velocity.x,
            _smoothTimeX
        );
        var newPosY = Mathf.SmoothDamp(
            _camera.transform.position.y,
            destination.y,
            ref _velocity.y,
            _smoothTimeY
        );
        _camera.transform.position = new Vector3(newPosX, newPosY, destination.z);
    }

    private Vector3 _CalculateDestination()
    {
        return _cameraTarget.Position;
    }

    private Vector3 _Bound(Vector3 pos)
    {
        return _bounds.Bound(pos, _camera.transform.position);
    }

    // エディタ上でカメラの移動範囲を視覚化するためのGizmosを描画
    void OnDrawGizmosSelected()
    {
        // デバッグ終了時などに参照が失われる場合があるため、カメラコンポーネントへの参照を再取得
        if (_camera == null)
        {
            _camera = GetComponentInChildren<Camera>();
            if (_camera == null)
                return;
        }

        _bounds.DrawGizmos(_camera, _camera.transform.position);
    }
}
