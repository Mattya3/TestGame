using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Constants;

[RequireComponent(typeof(GameManagerAccess))]
public class PlayersManager : MonoBehaviour, IPlayersCollection
{
    private List<Player> _players = new List<Player>();
    private GameManagerAccess _gameManager;

    public bool ArePlayersAlive { get; private set; } = true;

    private void Awake()
    {
        PlayersCollectionAccess.Register(this);
        PlayersCollectionReadonlyAccess.Register(this);

        _gameManager = GetComponent<GameManagerAccess>();
    }

    private void OnDestroy()
    {
        PlayersCollectionAccess.Unregister(this);
        PlayersCollectionReadonlyAccess.Unregister(this);
    }

    public void RegisterPlayer(Player player)
    {
        if (!_players.Contains(player))
            _players.Add(player);
        player.OnDied += (reason) =>
        {
            HandlePlayerDeath(player, reason);
        };
        player.OnGoal += (player) =>
        {
            HandlePlayerGoal(player);
        };
    }

    private void HandlePlayerDeath(Player deadPlayer, DeathReason deathReason)
    {
        if (!ArePlayersAlive)
            return;

        SetPlayersDead();
        FreezeAllPlayers();

        _gameManager.OnFailure();
    }

    private void HandlePlayerGoal(Player player)
    {
        if (!ArePlayersAlive)
            return;

        player.Freeze();

        if (!AllPlayersReachedGoal())
            return;

        _gameManager.OnSuccess();
    }

    private void SetPlayersDead()
    {
        ArePlayersAlive = false;
    }

    private void FreezeAllPlayers()
    {
        foreach (var player in _players)
        {
            player.Freeze();
        }
    }

    private bool AllPlayersReachedGoal()
    {
        return _players.Count > 0 && _players.All(p => p.HasReachedGoal);
    }

    public int Count => _players.Count;

    public List<Vector3> Positions => _players.Select(p => p.transform.position).ToList();

    public List<Bounds> BoundsList => _players.Select(p => p.Bounds).ToList();

    public List<Vector2> InputDirections => _players.Select(p => p.InputDirection).ToList();

    public void SetMoveController(IMoveController moveController)
    {
        foreach (var player in _players)
        {
            player.MoveController = moveController;
        }
    }
}
