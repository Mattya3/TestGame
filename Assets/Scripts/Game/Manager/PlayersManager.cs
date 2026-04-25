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

        // Findによってプレイヤを取得。プレイヤを動的に生成するようになったら、Findはやめる
        FindObjectsByType<Player>(FindObjectsSortMode.InstanceID).ToList().ForEach(player => _RegisterPlayer(player));
    }

    private void OnDestroy()
    {
        PlayersCollectionAccess.Unregister(this);
        PlayersCollectionReadonlyAccess.Unregister(this);
    }

    private void _RegisterPlayer(Player player)
    {
        if (!_players.Contains(player))
            _players.Add(player);
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
        if (!ArePlayersAlive)
            return;

        _SetPlayersDead();
        _FreezeAllPlayers();

        _gameManager.OnFailure();
    }

    private void _HandlePlayerGoal(Player player)
    {
        if (!ArePlayersAlive)
            return;

        player.Freeze();

        if (!_AllPlayersReachedGoal())
            return;

        _gameManager.OnSuccess();
    }

    private void _SetPlayersDead()
    {
        ArePlayersAlive = false;
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
