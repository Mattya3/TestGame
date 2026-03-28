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
    }

    void Start()
    {
        _players = GameManager.Instance.Players;

        _position = _CalculateCenter() + _offset;
        _shift.Start(_position);
    }

    void LateUpdate()
    {
        var center = _CalculateCenter();
        _shift.LateUpdate(center);
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

    public Vector3 Position => _position;
}
