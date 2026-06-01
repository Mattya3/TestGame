using System.Collections.Generic;
using UnityEngine;
using static Constants;

[RequireComponent(typeof(StageSceneContextAccess))]
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private MovementRuleManager _movementRuleManager;

    [SerializeField]
    private PlayersManager _playersManager;

    private StageSceneContextAccess _stageSceneContext;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _stageSceneContext = GetComponent<StageSceneContextAccess>();
    }

    private void Start()
    {
        _movementRuleManager.Initialize(_playersManager.Players);
    }

    public void HandleFailure()
    {
        GameEventTrigger.TriggerEvent(GameEvent.Failure);
        _stageSceneContext.OnStageRestarted();
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
