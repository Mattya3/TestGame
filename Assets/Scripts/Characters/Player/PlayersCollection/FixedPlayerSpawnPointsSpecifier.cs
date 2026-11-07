using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class FixedPlayerSpawnPointsSpecifier : MonoBehaviour, IPlayerSpawnPointsSpecifier
{
    [SerializeField]
    private List<Transform> _spawnPoints = new List<Transform>();

    private ReadOnlyCollection<Transform> _spawnPointsReadOnly;

    public ReadOnlyCollection<Transform> GetPlayerSpawnPoints()
    {
        _EnsureReadOnlyCollection();
        return _spawnPointsReadOnly;
    }

    private void _EnsureReadOnlyCollection()
    {
        if (_spawnPointsReadOnly == null)
        {
            _spawnPointsReadOnly = new ReadOnlyCollection<Transform>(_spawnPoints);
        }
    }
}
