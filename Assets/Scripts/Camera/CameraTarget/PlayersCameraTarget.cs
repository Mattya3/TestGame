using System.Collections.Generic;
using UnityEngine;

public class PlayersCameraTarget : MonoBehaviour, ICameraTarget
{
    [SerializeField]
    private Vector3 _offset;

    [SerializeField]
    private CameraTargetShift _shift = new CameraTargetShift();

    private IReadOnlyList<Player> _players;
    private Vector3 _position = Vector3.zero;

    void Awake()
    {
        if (_offset.z >= 0)
        {
            Debug.LogError("カメラのoffset.zは負の値でなければなりません", this);
            enabled = false;
            return;
        }
        _shift.Awake();
    }

    void Start()
    {
        _players = GameManager.Instance.Players;

        var center = _CalculateCenter();
        _shift.Start(center);
        _position = center + _offset + _shift.Shift;
    }

    void LateUpdate()
    {
        var center = _CalculateCenter();
        var distanceDampFactor = _MaxDistanceXY(_players);
        _shift.LateUpdate(center, distanceDampFactor);
        _position = center + _offset + _shift.Shift;
    }

    private Vector3 _CalculateCenter()
    {
        if (_players.Count == 0)
            return Vector3.zero;

        var sum = Vector3.zero;
        for (int i = 0; i < _players.Count; i++)
        {
            sum += _players[i].transform.position;
        }
        return sum / _players.Count;
    }
    private Vector2 _MaxDistanceXY(IReadOnlyList<Player> players)
    {
        var minPos = new Vector2(float.MaxValue, float.MaxValue);
        var maxPos = new Vector2(float.MinValue, float.MinValue);
        for (int i = 0; i < players.Count; i++)
        {
            var pos = players[i].transform.position;
            minPos = Vector2.Min(minPos, pos);
            maxPos = Vector2.Max(maxPos, pos);
        }
        return maxPos - minPos;
    }

    public Vector3 Position => _position;
}
