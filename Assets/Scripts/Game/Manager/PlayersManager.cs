using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using static Constants;

[RequireComponent(typeof(GameManagerMutableAccess))]
public class PlayersManager : MonoBehaviour, IPlayersCollection
{
    private List<Player> _players = new List<Player>();
    private GameManagerMutableAccess _gameManagerAccess;

    private List<Vector3> _positionsList = new List<Vector3>();
    private ReadOnlyCollection<Vector3> _positionsReadOnly;

    private List<Bounds> _boundsList = new List<Bounds>();
    private ReadOnlyCollection<Bounds> _boundsReadOnly;

    private List<Vector2> _inputDirectionsList = new List<Vector2>();
    private ReadOnlyCollection<Vector2> _inputDirectionsReadOnly;

    private List<bool> _aliveFlagsList = new List<bool>();
    private ReadOnlyCollection<bool> _aliveFlagsReadOnly;

    private IPlayerPrefabsSpecifier _playerPrefabsSpecifier;
    private IPlayerSpawnPointsSpecifier _playerSpawnPointsSpecifier;

    public bool AreAllPlayersAlive { get; private set; } = true;

    private void Awake()
    {
        _gameManagerAccess = GetComponent<GameManagerMutableAccess>();
        _playerPrefabsSpecifier = GetComponent<IPlayerPrefabsSpecifier>();
        _playerSpawnPointsSpecifier = GetComponent<IPlayerSpawnPointsSpecifier>();

        _positionsReadOnly = new ReadOnlyCollection<Vector3>(_positionsList);
        _boundsReadOnly = new ReadOnlyCollection<Bounds>(_boundsList);
        _inputDirectionsReadOnly = new ReadOnlyCollection<Vector2>(_inputDirectionsList);
        _aliveFlagsReadOnly = new ReadOnlyCollection<bool>(_aliveFlagsList);

        if (_playerPrefabsSpecifier == null)
        {
            Debug.LogError("IPlayerPrefabsSpecifierが見つかりません。");
            enabled = false;
            return;
        }
        if (_playerSpawnPointsSpecifier == null)
        {
            Debug.LogError("IPlayerSpawnPointsSpecifierが見つかりません。");
            enabled = false;
            return;
        }

        var playerPrefabs = _playerPrefabsSpecifier.GetPlayerPrefabs();
        if (playerPrefabs.Count == 0)
        {
            Debug.LogError("プレイヤのPrefabが指定されていません。");
            enabled = false;
            return;
        }
        var spawnPoints = _playerSpawnPointsSpecifier.GetPlayerSpawnPoints();
        if (spawnPoints.Count < playerPrefabs.Count)
        {
            Debug.LogError("プレイヤのSpawnPointが足りません。");
            enabled = false;
            return;
        }
        for (int i = 0; i < playerPrefabs.Count; i++)
        {
            var prefab = playerPrefabs[i];
            if (prefab == null)
            {
                Debug.LogError($"プレイヤのPrefabがnullです。インデックス: {i}");
                enabled = false;
                return;
            }
            var playerInstance = Instantiate(prefab, spawnPoints[i].position, spawnPoints[i].rotation);
            var playerComponent = playerInstance.GetComponent<Player>();
            if (playerComponent == null)
            {
                Debug.LogError($"プレイヤのPrefabにPlayerコンポーネントがアタッチされていません。インデックス: {i}");
                enabled = false;
                return;
            }
            playerComponent.SetupPlayerInput(i);
            _RegisterPlayer(playerComponent);
        }

        AccessComponent<IPlayersCollection>.RegisterReference(this);
    }

    private void OnDestroy()
    {
        AccessComponent<IPlayersCollection>.UnregisterReference(this);
    }

    private void _RegisterPlayer(Player player)
    {
        if (_players.Contains(player))
            return;

        _players.Add(player);
        _aliveFlagsList.Add(true);
        player.OnDied += (reason) =>
        {
            _HandlePlayerDeath(player, reason);
        };
        player.OnGoal += (player) =>
        {
            _HandlePlayerGoal(player);
        };
    }

    private void _HandlePlayerDeath(Player deadPlayer, DeathReason deathReason)
    {
        if (!AreAllPlayersAlive)
            return;

        _SetPlayersDead();
        _FreezeAllPlayers();
        _gameManagerAccess.HandleFailure();
    }

    private void _HandlePlayerGoal(Player player)
    {
        if (!AreAllPlayersAlive)
            return;

        player.Freeze();

        if (!_AllPlayersReachedGoal())
            return;

        _gameManagerAccess.HandleSuccess();
    }

    private void _SetPlayersDead()
    {
        AreAllPlayersAlive = false;
    }

    private void _FreezeAllPlayers()
    {
        foreach (var player in _players)
        {
            player.EnterFrozenState();
        }
    }

    private bool _AllPlayersReachedGoal()
    {
        return _players.Count > 0 && _players.All(player => player.IsInGoalState);
    }

    public int Count => _players.Count;

    public ReadOnlyCollection<Vector3> Positions
    {
        get
        {
            _positionsList.Clear();
            foreach (var player in _players)
            {
                _positionsList.Add(player.transform.position);
            }
            return _positionsReadOnly;
        }
    }

    public ReadOnlyCollection<Bounds> BoundsList
    {
        get
        {
            _boundsList.Clear();
            foreach (var player in _players)
            {
                _boundsList.Add(player.Bounds);
            }
            return _boundsReadOnly;
        }
    }

    public ReadOnlyCollection<Vector2> InputDirections
    {
        get
        {
            _inputDirectionsList.Clear();
            foreach (var player in _players)
            {
                _inputDirectionsList.Add(player.InputDirection);
            }
            return _inputDirectionsReadOnly;
        }
    }

    public ReadOnlyCollection<bool> AliveFlags
    {
        get
        {
            _aliveFlagsList.Clear();
            foreach (var player in _players)
            {
                _aliveFlagsList.Add(player.IsAlive);
            }
            return _aliveFlagsReadOnly;
        }
    }
}
