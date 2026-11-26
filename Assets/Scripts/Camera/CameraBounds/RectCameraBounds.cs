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

    public override Vector3 Bound(Vector3 pos, Vector3 originalPos, Camera camera)
    {
        var boundPos = base.Bound(pos, originalPos, camera);

        var cameraHalfHeight = camera.orthographicSize;
        var cameraHalfWidth = cameraHalfHeight * camera.aspect;

        return new Vector3(
            Mathf.Clamp(boundPos.x, _leftBound + cameraHalfWidth, _rightBound - cameraHalfWidth),
            Mathf.Clamp(boundPos.y, _bottomBound + cameraHalfHeight, _topBound - cameraHalfHeight),
            boundPos.z
        );
    }

    public override void DrawGizmos(Camera camera, Vector3 originalPos)
    {
        base.DrawGizmos(camera, originalPos);

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
