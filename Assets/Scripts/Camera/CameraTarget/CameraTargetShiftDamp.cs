using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraTargetShiftDamp
{
    [SerializeField]
    private Collider2D _leftCollider;

    [SerializeField]
    private Collider2D _rightCollider;

    private const float MARGIN = 1e-2f;
    private const float EPSILON = 1e-3f;

    public Vector2 CalculateDamp(PlayersCollectionReadonlyAccess players)
    {
        var distanceLimits = _CalculateDistanceLimits();

        var spaceXY = _SpaceXY(players);
        var scaledSpaceXY = Vector2.Scale(
            spaceXY - Vector2.one * MARGIN,
            Vector2.Max(
                new Vector2(0.5f / distanceLimits.x, 0.5f / distanceLimits.y),
                Vector2.one * EPSILON
            )
        );

        var clippedSpaceXY = Vector2.Max(Vector2.Min(Vector2.one, scaledSpaceXY), Vector2.zero);
        return _CalculateDampCurve(clippedSpaceXY);
    }

    // プレイヤーの位置とカメラの左右のコライダーとのスペースを計算する。スペースが十分にあるほど1に近づき、スペースがないほど0に近づく。
    private Vector2 _SpaceXY(PlayersCollectionReadonlyAccess players)
    {
        if (players.Count == 0)
            return Vector2.zero;

        if (_leftCollider == null || _rightCollider == null)
            return float.PositiveInfinity * Vector2.one;

        var boundsList = players.BoundsList;
        var minPos = new Vector2(float.MaxValue, float.MaxValue);
        var maxPos = new Vector2(float.MinValue, float.MinValue);
        for (int i = 0; i < boundsList.Count; i++)
        {
            var bounds = boundsList[i];
            minPos = Vector2.Min(minPos, bounds.center - bounds.extents);
            maxPos = Vector2.Max(maxPos, bounds.center + bounds.extents);
        }

        var spaceBehindX = Mathf.Min(
            minPos.x - _leftCollider.bounds.max.x,
            _rightCollider.bounds.min.x - maxPos.x
        );
        return new Vector2(spaceBehindX, float.PositiveInfinity);
    }

    private Vector2 _CalculateDampCurve(Vector2 val)
    {
        // パターン1: 線形減衰
        // return val;

        // パターン2:  二次関数的に減衰させる。1付近で勾配が連続になる。
        return Vector2.Scale(val, Vector2.one * 2.0f - val);

        // パターン3: 三次関数。0と1付近で勾配が連続になる。
        //var squared = Vector2.Scale(val, val);
        //var cubed = Vector2.Scale(val, squared);
        //return -0.5f * cubed + 1.5f * squared;
    }

    private Vector2 _CalculateDistanceLimits()
    {
        var horizontalLimit = _CalculateHorizontalCollidersDistance();
        return new Vector2(horizontalLimit, float.PositiveInfinity); // Yはひとまず考慮しない
    }

    private float _CalculateHorizontalCollidersDistance()
    {
        // カメラを拡大縮小したときに対応するため毎回計算
        if (_leftCollider == null || _rightCollider == null)
            return float.PositiveInfinity;
        return _rightCollider.bounds.min.x - _leftCollider.bounds.max.x;
    }
}
