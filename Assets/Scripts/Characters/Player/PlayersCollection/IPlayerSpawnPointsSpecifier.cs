using UnityEngine;
using System.Collections.ObjectModel;

public interface IPlayerSpawnPointsSpecifier
{
    ReadOnlyCollection<Transform> GetPlayerSpawnPoints();
}