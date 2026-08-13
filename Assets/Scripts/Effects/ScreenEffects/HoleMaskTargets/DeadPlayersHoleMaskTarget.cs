using UnityEngine;
using System.Collections.ObjectModel;
using System.Collections.Generic;

[RequireComponent(typeof(PlayersCollectionReadonlyAccess))]
[RequireComponent(typeof(CameraReadonlyAccess))]
public class DeadPlayersHoleMaskTarget : MonoBehaviour, IHoleMaskTarget
{
    private PlayersCollectionReadonlyAccess _playersCollectionAccess;
    private CameraReadonlyAccess _cameraAccess;

    private List<bool> _enabledList = new List<bool>();
    private ReadOnlyCollection<bool> _enabledReadOnly;

    private List<Vector3> _screenPositionsList = new List<Vector3>();
    private ReadOnlyCollection<Vector3> _screenPositionsReadOnly;

    private void Awake()
    {
        _playersCollectionAccess = GetComponent<PlayersCollectionReadonlyAccess>();
        _cameraAccess = GetComponent<CameraReadonlyAccess>();

        _enabledReadOnly = new ReadOnlyCollection<bool>(_enabledList);
        _screenPositionsReadOnly = new ReadOnlyCollection<Vector3>(_screenPositionsList);
    }

    public ReadOnlyCollection<bool> AreEnabled
    {
        get
        {
            var numPlayers = _playersCollectionAccess.Count;
            var aliveFlags = _playersCollectionAccess.AliveFlags;
            _enabledList.Clear();
            for (int i = 0; i < numPlayers; i++)
            {
                _enabledList.Add(!aliveFlags[i]);
            }
            return _enabledReadOnly;
        }
    }

    public ReadOnlyCollection<Vector3> ScreenPositions
    {
        get
        {
            var numPlayers = _playersCollectionAccess.Count;
            var positions = _playersCollectionAccess.Positions;
            _screenPositionsList.Clear();
            for (int i = 0; i < numPlayers; i++)
            {
                var worldPosition = positions[i];
                var screenPosition = _cameraAccess.WorldToScreenPoint(worldPosition);
                _screenPositionsList.Add(screenPosition);
            }
            return _screenPositionsReadOnly;
        }
    }
}
