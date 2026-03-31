using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraTargetShiftDamp
{
    [SerializeField]
    private Vector2 _distanceDampThreshold;

    [SerializeField]
    private Collider2D _leftCollider;

    [SerializeField]
    private Collider2D _rightCollider;

    public Vector2 CalculateDamp(IReadOnlyList<Player> players)
    {
        var maxDistanceXY = _MaxDistanceXY(players);
        var exceeds = Vector2.Max(Vector2.zero, maxDistanceXY - _distanceDampThreshold);
        var ofsetDistanceLimits = _CalculateDistanceLimits() - _distanceDampThreshold;
        var scaledExceeds = Vector2.Scale(exceeds, new Vector2(1.0f / ofsetDistanceLimits.x, 1.0f / ofsetDistanceLimits.y));

        var curvedExceeds = _CalculateExceedsCurve(scaledExceeds);
        return Vector2.one - Vector2.Min(Vector2.one, curvedExceeds);
    }

    private Vector2 _MaxDistanceXY(IReadOnlyList<Player> players)
    {
        var minPos = new Vector2(float.MaxValue, float.MaxValue);
        var maxPos = new Vector2(float.MinValue, float.MinValue);
        for (int i = 0; i < players.Count; i++)
        {
            var bounds = players[i].Bounds;
            minPos = Vector2.Min(minPos, bounds.center - bounds.extents);
            maxPos = Vector2.Max(maxPos, bounds.center + bounds.extents);
        }
        return maxPos - minPos;
    }

    private Vector2 _CalculateExceedsCurve(Vector2 scaledExceeds)
    {
        // パターン1: 線形減衰
        // return scaledExceeds;

        // パターン2:  二次関数的に減衰させる。0付近で勾配が連続になる。
        return Vector2.Scale(scaledExceeds, scaledExceeds);

        // パターン3: 三次関数。0と1付近で勾配が連続になる。
        //var cubed = Vector2.Scale(scaledExceeds, Vector2.Scale(scaledExceeds, scaledExceeds));
        //var squared = Vector2.Scale(scaledExceeds, scaledExceeds);
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
