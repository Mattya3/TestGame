using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameConstants;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool ArePlayersAlive { get; private set; } = true;

    private event Action _onFailure;
    private event Action _onSuccess;

    [SerializeField]
    private ScreenEffectsController _screenEffectsController;

    [SerializeField]
    private MovementRuleEffect _movementRuleEffect;

    private IGameMoveController _moveController;

    private List<Player> _players = new List<Player>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _moveController = GameMoveControllerFactory.Create(_movementRuleEffect, _players);
    }

    public Vector2 ConvertInputDirection(Vector2 rawInput) =>
        _moveController.ConvertInputDirection(rawInput);

    public void RegisterEventAction(GameEvent gameEvent, Action eventAction)
    {
        switch (gameEvent)
        {
            case GameEvent.Failure:
                _onFailure += eventAction;
                break;
            case GameEvent.Success:
                _onSuccess += eventAction;
                break;
            default:
                Debug.LogError($"Unhandled GameEvent value in RegisterEventAction: {gameEvent}");
                throw new ArgumentOutOfRangeException(nameof(gameEvent), gameEvent, null);
        }
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

        _onFailure?.Invoke();
        _RestartStage();
    }

    public void HandlePlayerGoal(Player player)
    {
        // 2Pがいるときの処理はのちのち実装。とりあえず今はゴールしたプレイヤを止めるだけ。
        player.Freeze();

        // 全員がゴールしたときの処理
        if (_AllPlayersHaveReachedGoal())
        {
            _onSuccess?.Invoke();

            // ゴール演出
            _screenEffectsController.PlaySuccessEffect(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
        }
    }

    private bool _AllPlayersHaveReachedGoal() => _players.All(player => player.HasReachedGoal);

    private void _RestartStage()
    {
        _screenEffectsController.PlayFailureEffect(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }
}
