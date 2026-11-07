using System.Collections.ObjectModel;
using UnityEngine;

public interface IPlayerPrefabsSpecifier
{
    ReadOnlyCollection<GameObject> GetPlayerPrefabs();
}
