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

    public bool AreAllPlayersAlive { get; private set; } = true;

    private void Awake()
    {
        AccessComponent<IPlayersCollection>.RegisterReference(this);

        // Findによってプレイヤを取得。プレイヤを動的に生成するようになったら、Findはやめる
        foreach (var player in FindObjectsByType<Player>(FindObjectsSortMode.InstanceID))
            _RegisterPlayer(player);

        _gameManagerAccess = GetComponent<GameManagerMutableAccess>();

        _positionsReadOnly = new ReadOnlyCollection<Vector3>(_positionsList);
        _boundsReadOnly = new ReadOnlyCollection<Bounds>(_boundsList);
        _inputDirectionsReadOnly = new ReadOnlyCollection<Vector2>(_inputDirectionsList);
        _aliveFlagsReadOnly = new ReadOnlyCollection<bool>(_aliveFlagsList);
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

        _SetPlayerAliveFlag(deadPlayer, false);
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

    private void _SetPlayerAliveFlag(Player player, bool isAlive)
    {
        int index = _players.IndexOf(player);
        if (index >= 0)
        {
            _aliveFlagsList[index] = isAlive;
        }
    }

    private void _SetPlayersDead()
    {
        AreAllPlayersAlive = false;
    }

    private void _FreezeAllPlayers()
    {
        foreach (var player in _players)
        {
            player.Freeze();
        }
    }

    private bool _AllPlayersReachedGoal()
    {
        return _players.Count > 0 && _players.All(p => p.HasReachedGoal);
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

    public ReadOnlyCollection<bool> AliveFlags => _aliveFlagsReadOnly;

    public void SetMoveController(IMoveController moveController)
    {
        foreach (var player in _players)
        {
            player.MoveController = moveController;
        }
    }
}
