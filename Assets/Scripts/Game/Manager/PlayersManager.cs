using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameConstants;

public class PlayersManager : MonoBehaviour
{
    private List<Player> _players = new List<Player>();

    public bool ArePlayersAlive { get; private set; } = true;

    public IReadOnlyList<Player> Players => _players;

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
}
