using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class FixedPlayerPrefabsSpecifier : MonoBehaviour, IPlayerPrefabsSpecifier
{
    [SerializeField]
    private List<GameObject> _playerPrefabs;

    private ReadOnlyCollection<GameObject> _playerPrefabsReadOnly;

    public ReadOnlyCollection<GameObject> GetPlayerPrefabs()
    {
        _EnsureReadOnlyCollection();
        return _playerPrefabsReadOnly;
    }

    private void _EnsureReadOnlyCollection()
    {
        if (_playerPrefabsReadOnly == null)
        {
            _playerPrefabsReadOnly = new ReadOnlyCollection<GameObject>(_playerPrefabs);
        }
    }
}
