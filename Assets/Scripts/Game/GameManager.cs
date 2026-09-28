using UnityEngine;
using static Constants;

[RequireComponent(typeof(GameEventTriggerAccess))]
public class GameManager : MonoBehaviour, IGameManager
{
    [SerializeField]
    private MovementRuleManager _movementRuleManager;

    private GameEventTriggerAccess _gameEventTriggerAccess;

    private void Awake()
    {
        _gameEventTriggerAccess = GetComponent<GameEventTriggerAccess>();
        AccessComponent<IGameManager>.RegisterReference(this);
    }

    private void OnDestroy()
    {
        AccessComponent<IGameManager>.UnregisterReference(this);
    }

    private void Start()
    {
        _movementRuleManager.Initialize();
    }

    public void HandlePlayStart()
    {
        _gameEventTriggerAccess.TriggerEventActions(GameEvent.GamePlayStart);
    }

    public void HandleFailure()
    {
        _gameEventTriggerAccess.TriggerEventActions(GameEvent.Failure);
    }

    public void HandleSuccess()
    {
        _gameEventTriggerAccess.TriggerEventActions(GameEvent.Success);
    }

    public void HandleSceneEnd()
    {
        _gameEventTriggerAccess.TriggerEventActions(GameEvent.SceneEnd);
    }
}
