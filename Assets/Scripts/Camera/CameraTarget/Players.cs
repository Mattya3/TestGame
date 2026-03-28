using System.Collections.Generic;
using UnityEngine;

namespace CameraTarget
{
    public class Players : ICameraTarget
    {
        private const float CAMERA_MARGIN = 1e-2f;

        private IReadOnlyList<Player> _players;

        public Players(IReadOnlyList<Player> players)
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
}
