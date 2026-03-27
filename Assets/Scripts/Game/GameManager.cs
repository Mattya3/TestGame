using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Constants;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private SceneTransitionManager _sceneTransitionManager;

    [SerializeField]
    private MovementRuleManager _movementRuleManager;

    [SerializeField]
    private PlayersManager _playersManager;

    private event Action _onFailure;
    private event Action _onSuccess;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _movementRuleManager.Initialize(_playersManager.Players);
    }

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
    /// プレイヤーが死亡したときに呼び出されます。
    /// </summary>
    public void HandleFailure()
    {
        _onFailure?.Invoke();
        _sceneTransitionManager.RestartStage();
    }

    public void HandleSuccess()
    {
        _onSuccess?.Invoke();
        _sceneTransitionManager.CompleteStage();
    }

    public bool ArePlayersAlive() => _playersManager.ArePlayersAlive;
}
