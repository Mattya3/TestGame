using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameConstants;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool ArePlayersAlive { get; private set; } = true;

    public event Action OnPlayerDied;

    [SerializeField]
    private UIEffectController _uiEffectController;

    private List<Player> _players = new List<Player>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    /// <summary>
    /// プレイヤーを登録します。
    /// </summary>
    public void RegisterPlayer(Player player)
    {
        if (!_players.Contains(player))
            _players.Add(player);
    }

    /// <summary>
    /// プレイヤーが死亡したときに呼び出されます。
    /// </summary>
    public void HandlePlayerDeath(Player deadPlayer, DeathReason deathReason)
    {
        if (!ArePlayersAlive)
            return;
        ArePlayersAlive = false;

        for (int i = 0; i < _players.Count; i++)
        {
            _players[i].Freeze();
        }

        OnPlayerDied?.Invoke();
        _RestartStage();
    }

    public void HandlePlayerGoal(Player player)
    {
        // 2Pがいるときの処理はのちのち実装。とりあえず今はゴールしたプレイヤを止めるだけ。
        for (int i = 0; i < _players.Count; i++)
        {
            _players[i].Freeze();
        }
        _uiEffectController.PlayGoalEffect(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }

    private void _RestartStage()
    {
        _uiEffectController.PlayDeathEffect(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }
}
