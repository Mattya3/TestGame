using UnityEngine;
using System.Collections.ObjectModel;

public interface IPlayerPrefabsSpecifier
{
    ReadOnlyCollection<GameObject> GetPlayerPrefabs();
}