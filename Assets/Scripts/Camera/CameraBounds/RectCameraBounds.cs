using System;
using UnityEngine;

[Serializable]
public class RectCameraBounds : CameraBoundsBase
{
    [SerializeField]
    private float _leftBound = float.NegativeInfinity; // X座標の最小値

    [SerializeField]
    private float _rightBound = float.PositiveInfinity; // X座標の最大値

    [SerializeField]
    private float _bottomBound = float.NegativeInfinity; // Y座標の最小値

    [SerializeField]
    private float _topBound = float.PositiveInfinity; // Y座標の最大値

    protected override void Awake()
    {
        if (!_IsConfigurationValid())
        {
            enabled = false;
            return;
        }
        base.Awake();
    }

    public bool HasReversedBounds()
    {
        return _leftBound > _rightBound || _bottomBound > _topBound;
    }

    public bool HasNaN()
    {
        return float.IsNaN(_leftBound)
            || float.IsNaN(_rightBound)
            || float.IsNaN(_bottomBound)
            || float.IsNaN(_topBound);
    }

    public bool HasInfinity()
    {
        return float.IsInfinity(_leftBound)
            || float.IsInfinity(_rightBound)
            || float.IsInfinity(_bottomBound)
            || float.IsInfinity(_topBound);
    }

    private bool _IsConfigurationValid()
    {
        if (HasReversedBounds())
        {
            Debug.LogError("カメラ制約の最小値は最大値より小さくなければなりません", this);
            return false;
        }
        if (HasNaN())
        {
            Debug.LogError("カメラの制約に無効な値が含まれています", this);
            return false;
        }
        return true;
    }

    public override Vector3 Bound(Vector3 pos, Vector3 originalPos, Camera camera)
    {
        var boundPos = base.Bound(pos, originalPos, camera);

        var cameraHalfHeight = camera.orthographicSize;
        var cameraHalfWidth = cameraHalfHeight * camera.aspect;

        var leftLimit = _leftBound + cameraHalfWidth;
        var rightLimit = _rightBound - cameraHalfWidth;
        var bottomLimit = _bottomBound + cameraHalfHeight;
        var topLimit = _topBound - cameraHalfHeight;

        if (leftLimit > rightLimit)
            leftLimit = rightLimit = (leftLimit + rightLimit) / 2f;
        if (bottomLimit > topLimit)
            bottomLimit = topLimit = (bottomLimit + topLimit) / 2f;

        return new Vector3(
            Mathf.Clamp(boundPos.x, leftLimit, rightLimit),
            Mathf.Clamp(boundPos.y, bottomLimit, topLimit),
            boundPos.z
        );
    }

    public override void DrawGizmos()
    {
        base.DrawGizmos();

        if (!_CanDrawGizmos())
            return;

        var cameraSize = new Vector3(_rightBound - _leftBound, _topBound - _bottomBound, 1);
        var cameraCenter = new Vector3(
            (_leftBound + _rightBound) / 2,
            (_bottomBound + _topBound) / 2,
            0
        );
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(cameraCenter, cameraSize);
    }

    private bool _CanDrawGizmos()
    {
        return !HasReversedBounds() && !HasNaN() && !HasInfinity();
    }
}
