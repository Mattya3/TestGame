using System.Collections.Generic;
using UnityEngine;

public class PlayersCameraTarget : MonoBehaviour, ICameraTarget
{
    [SerializeField]
    private Vector3 _offset;

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
        _position = _offset;
    }

    void LateUpdate()
    {
        if (_players.Count == 0)
            return;

        var sum = Vector3.zero;
        for (int i = 0; i < _players.Count; i++)
        {
            sum += _players[i].transform.position;
        }
        _position = sum / _players.Count + _offset;
    }

    public Vector3 Position => _position;
}
