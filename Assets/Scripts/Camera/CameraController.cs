using UnityEngine;

[RequireComponent(typeof(CameraBoundsReadonlyAccess))]
[RequireComponent(typeof(ShakeEffectsPlayer))]
public class CameraController : MonoBehaviour, IAccessTarget
{
    [Header("Reference")]
    [SerializeField]
    private CameraCollider _colliderRoot;

    [Header("Smoothing")]
    [SerializeField, Min(0f)]
    private float _smoothTimeX = 0.1f;

    [SerializeField, Min(0f)]
    private float _smoothTimeY = 0.1f;

    private Camera _camera; // カメラコンポーネントへの参照
    private CameraTargetsStack _targetsStack; // カメラターゲットのスタック
    private Vector3 _smoothedPosition; // スムージングされたカメラの位置
    private Vector3 _velocity = Vector3.zero; // カメラの現在の速度
    private ShakeEffectsPlayer _shakeEffectsPlayer; // カメラの揺れ効果を管理するコンポーネント
    private CameraBoundsReadonlyAccess _boundsAccess; // カメラ制約への読み取り専用アクセス

    private bool _isInitialized = false; // 初期化済みかどうかのフラグ

    void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
        _targetsStack = new CameraTargetsStack(GetComponentsInChildren<ICameraTarget>());
        if (!_IsConfigurationValid())
        {
            enabled = false;
            return;
        }
        _shakeEffectsPlayer = GetComponent<ShakeEffectsPlayer>();
        _boundsAccess = GetComponent<CameraBoundsReadonlyAccess>();

        AccessComponent<CameraController>.RegisterReference(this);
    }

    void OnDestroy()
    {
        AccessComponent<CameraController>.UnregisterReference(this);
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
        if (_targetsStack.IsEmpty)
        {
            Debug.LogError(
                "CameraControllerには少なくとも1つのICameraTargetを実装したコンポーネントが必要です",
                this
            );
            return false;
        }
        return true;
    }

    void Start()
    {
        EnsureInitialized();
    }

    private void _Initialize()
    {
        _targetsStack.Start();

        _colliderRoot.AlignCollidersToViewportEdges(_camera);
        _colliderRoot.EnableColliders(_targetsStack.AreCollidersEnabled);

        // カメラの初期位置を設定
        var destination = _CalculateDestination();
        var boundedDestination = _Bound(destination);

        _colliderRoot.transform.position = boundedDestination;
        _smoothedPosition = boundedDestination + _targetsStack.AnimatedShift;
        _camera.transform.position = _smoothedPosition;

        _isInitialized = true;
    }

    public void EnsureInitialized()
    {
        if (_isInitialized)
            return;

        _Initialize();
    }

    public void PushTarget(ICameraTarget target)
    {
        _targetsStack.Push(target);
        _colliderRoot.EnableColliders(_targetsStack.AreCollidersEnabled);
    }

    public void PopTarget()
    {
        _targetsStack.Pop();
        _colliderRoot.EnableColliders(_targetsStack.AreCollidersEnabled);
    }

    public void PlayShake(ShakeEffect shakeEffect, bool playInUnscaledTime, float playbackSpeedRate)
    {
        _shakeEffectsPlayer.Play(shakeEffect, playInUnscaledTime, playbackSpeedRate);
    }

    public float AspectRatio => _camera.aspect;

    public Rect ViewportRect => _camera.rect;

    public Vector3 WorldToViewportPoint(Vector3 worldPosition)
    {
        return _camera.WorldToViewportPoint(worldPosition);
    }

    public Vector3 UnoffsetPosition => _smoothedPosition;

    private void FixedUpdate()
    {
        _targetsStack.Update();
        _colliderRoot.EnableColliders(_targetsStack.AreCollidersEnabled);

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
        var targetAnimatedShift = _targetsStack.AnimatedShift;
        var destination = _colliderRoot.transform.position + targetAnimatedShift;

        var smoothedPosX = Mathf.SmoothDamp(
            _smoothedPosition.x,
            destination.x,
            ref _velocity.x,
            _smoothTimeX
        );
        var smoothedPosY = Mathf.SmoothDamp(
            _smoothedPosition.y,
            destination.y,
            ref _velocity.y,
            _smoothTimeY
        );
        _smoothedPosition = new Vector3(smoothedPosX, smoothedPosY, destination.z);

        var shakeOffset = _shakeEffectsPlayer.CurrentShakeOffset;

        _camera.transform.position = new Vector3(
            _smoothedPosition.x + shakeOffset.x,
            _smoothedPosition.y + shakeOffset.y,
            _smoothedPosition.z
        );
    }

    private Vector3 _CalculateDestination()
    {
        return _targetsStack.Position;
    }

    private Vector3 _Bound(Vector3 pos)
    {
        return _boundsAccess.Bound(pos, _camera.transform.position, _camera);
    }
}
