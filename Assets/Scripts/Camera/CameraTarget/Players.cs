using System.Collections.Generic;
using UnityEngine;

namespace CameraTarget
{
    public class Players : ICameraTarget
    {
        private IReadOnlyList<Player> _players;

        public Players(IReadOnlyList<Player> players)
        {
            _players = players;
        }

        public Vector3 Position()
        {
            var sum = Vector3.zero;
            for (int i = 0; i < _players.Count; i++)
            {
                sum += _players[i].transform.position;
            }
            return sum / _players.Count;
        }
    }
}
