using System.Collections.Generic;
using UnityEngine;

public class PlayersCameraTarget : ICameraTarget
{
    private IReadOnlyList<Player> _players;

    public PlayersCameraTarget(IReadOnlyList<Player> players)
    {
        _players = players;
    }

    public Vector3 Position()
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
}
