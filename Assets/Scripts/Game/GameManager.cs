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
        AccessComponent<IGameManager>.RegisterReference(this);
        _gameEventTriggerAccess = GetComponent<GameEventTriggerAccess>();
    }

    private void OnDestroy()
    {
        AccessComponent<IGameManager>.UnregisterReference(this);
    }

    private void Start()
    {
        _movementRuleManager.Initialize();
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
