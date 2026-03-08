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
    private bool _freezeX = false; // X軸の移動をロックするかどうか

    [SerializeField]
    private bool _freezeY = false; // Y軸の移動をロックするかどうか

    [SerializeField]
    private float _leftBound = float.NegativeInfinity; // カメラのX座標の最小値

    [SerializeField]
    private float _rightBound = float.PositiveInfinity; // カメラのX座標の最大値

    [SerializeField]
    private float _bottomBound = float.NegativeInfinity; // カメラのY座標の最小値

    [SerializeField]
    private float _topBound = float.PositiveInfinity; // カメラのY座標の最大値

    private Rect _boundingRect => Rect.MinMaxRect(_leftBound, _bottomBound, _rightBound, _topBound); // カメラの移動範囲を表す矩形

    // == Component References ==
    private Camera _camera; // カメラコンポーネントへの参照

    // == Unity Event Functions ==

    // Awake is called when the script instance is being loaded
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
        if (_target == null)
        {
            Debug.LogError("カメラのtargetが設定されていません", this);
            return false;
        }
        if (_offset.z >= 0)
        {
            Debug.LogError("カメラのoffset.zは負の値でなければなりません", this);
            return false;
        }
        if (_leftBound > _rightBound || _bottomBound > _topBound)
        {
            Debug.LogError("カメラ制約の最小値は最大値より小さくなければなりません", this);
            return false;
        }
        if (float.IsNaN(_leftBound) || float.IsNaN(_rightBound) || float.IsNaN(_bottomBound) || float.IsNaN(_topBound))
        {
            Debug.LogError("カメラの制約に無効な値が含まれています", this);
            return false;
        }
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        // カメラの初期位置を設定
        var destination = _CalculateDestination();
        transform.position = destination;
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
        destination.x = _freezeX ? transform.position.x : Mathf.Clamp(destination.x, _leftBound, _rightBound);
        destination.y = _freezeY ? transform.position.y : Mathf.Clamp(destination.y, _bottomBound, _topBound);
        return destination;
    }

    // エディタ上でカメラの移動範囲を視覚化するためのGizmosを描画
    void OnDrawGizmosSelected()
    {
        if (!_AreConstraintsVisualizable())
            return;

        // デバッグ終了時などに参照が失われる場合があるため、カメラコンポーネントへの参照を再取得
        if (_camera == null)
        {
            _camera = GetComponent<Camera>();
            if (_camera == null)
                return;
        }

        var halfHeight = _camera.orthographicSize;
        var halfWidth = halfHeight * _camera.aspect;

        var visualMinX = (_freezeX ? transform.position.x : _leftBound) - halfWidth;
        var visualMaxX = (_freezeX ? transform.position.x : _rightBound) + halfWidth;
        var visualMinY = (_freezeY ? transform.position.y : _bottomBound) - halfHeight;
        var visualMaxY = (_freezeY ? transform.position.y : _topBound) + halfHeight;

        var cameraSize = new Vector3((visualMaxX - visualMinX), (visualMaxY - visualMinY), 1);
        var cameraCenter = new Vector3(
            (visualMinX + visualMaxX) / 2,
            (visualMinY + visualMaxY) / 2,
            0
        );
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(cameraCenter, cameraSize);
    }

    private bool _AreConstraintsValid()
    {
        // 最小値が最大値より小さいことを確認
        if (_leftBound > _rightBound || _bottomBound > _topBound)
        {
            Debug.LogError("カメラ制約の最小値は最大値より小さくなければなりません", this);
            return false;
        }

        // NaNが含まれていないことを確認
        if (float.IsNaN(_leftBound) || float.IsNaN(_rightBound) || float.IsNaN(_bottomBound) || float.IsNaN(_topBound))
        {
            Debug.LogError("カメラの制約に無効な値が含まれています", this);
            return false;
        }

        return true;
    }

    private bool _AreConstraintsVisualizable()
    {
        if (!_AreConstraintsValid())
            return false;

        if (
            float.IsInfinity(_leftBound)
            || float.IsInfinity(_rightBound)
            || float.IsInfinity(_bottomBound)
            || float.IsInfinity(_topBound)
        )
        {
            Debug.LogWarning(
                "カメラの制約が無限大を含んでいるため、Gizmosで視覚化できません",
                this
            );
            return false;
        }

        return true;
    }
}
