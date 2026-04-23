using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Constants;

public class PlayersManager : MonoBehaviour, IPlayersCollection
{
    private List<Player> _players = new List<Player>();

    public bool ArePlayersAlive { get; private set; } = true;

    public IReadOnlyList<Player> Players => _players;

    private void Awake()
    {
        PlayersCollectionAccess.Register(this);
        PlayersCollectionReadonlyAccess.Register(this);
    }

    private void OnDestroy()
    {
        PlayersCollectionAccess.Unregister(this);
        PlayersCollectionReadonlyAccess.Unregister(this);
    }

    private void OnEnable()
    {
        Player.OnCreated += RegisterPlayer;
    }

    public void RegisterPlayer(Player player)
    {
        if (!_players.Contains(player))
            _players.Add(player);
        player.OnDied += (reason) =>
        {
            if (ArePlayersAlive)
                HandlePlayerDeath(player, reason);
        };
        player.OnGoal += (player) =>
        {
            if (ArePlayersAlive)
                HandlePlayerGoal(player);
        };
    }

    public void HandlePlayerDeath(Player deadPlayer, DeathReason deathReason)
    {
        if (!ArePlayersAlive)
            return;

        SetPlayersDead();
        FreezeAllPlayers();

        GameManager.Instance.HandleFailure();
    }

    public void HandlePlayerGoal(Player player)
    {
        player.Freeze();

        if (!AllPlayersReachedGoal())
            return;

        GameManager.Instance.HandleSuccess();
    }

    public void SetPlayersDead()
    {
        ArePlayersAlive = false;
    }

    public void FreezeAllPlayers()
    {
        foreach (var player in _players)
        {
            player.Freeze();
        }
    }

    public bool AllPlayersReachedGoal()
    {
        return _players.Count > 0 && _players.All(p => p.HasReachedGoal);
    }

    public int Count => _players.Count;

    public List<Vector3> Positions => _players.Select(p => p.transform.position).ToList();

    public List<Bounds> BoundsList => _players.Select(p => p.Bounds).ToList();
}
