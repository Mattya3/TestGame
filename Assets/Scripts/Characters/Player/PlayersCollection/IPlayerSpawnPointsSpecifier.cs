using System.Collections.ObjectModel;
using UnityEngine;

public interface IPlayerSpawnPointsSpecifier
{
    ReadOnlyCollection<Transform> GetPlayerSpawnPoints();
}
