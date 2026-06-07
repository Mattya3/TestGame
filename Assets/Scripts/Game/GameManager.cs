using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class GameManager : MonoBehaviour
{
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
    }

    public void HandleSuccess()
    {
        GameEventTrigger.TriggerEvent(GameEvent.Success);
    }

    public void HandleSceneEnd()
    {
        GameEventTrigger.TriggerEvent(GameEvent.SceneEnd);
        GameEventTrigger.ResetEvents();
    }

    public IReadOnlyList<Player> Players => _playersManager.Players;
}
