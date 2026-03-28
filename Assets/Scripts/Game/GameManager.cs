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

    public void HandleFailure()
    {
        GameEventTrigger.TriggerEvent(GameEvent.Failure);
        GameEventTrigger.ResetEvents();
        _sceneTransitionManager.RestartStage();
    }

    public void HandleSuccess()
    {
        GameEventTrigger.TriggerEvent(GameEvent.Success);
        GameEventTrigger.ResetEvents();
        _sceneTransitionManager.CompleteStage();
    }

    public IReadOnlyList<Player> Players()
    {
        return _playersManager.Players;
    }
}
