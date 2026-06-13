using UnityEngine;
using static Constants;

[RequireComponent(typeof(GameEventTriggerAccess))]
public class GameManager : MonoBehaviour, IGameManager
{
    [SerializeField]
    private MovementRuleManager _movementRuleManager;

    private GameEventTriggerAccess _gameEventTrigger;

    private void Awake()
    {
        AccessComponent<IGameManager>.Register(this);
        _gameEventTrigger = GetComponent<GameEventTriggerAccess>();
    }

    private void OnDestroy()
    {
        AccessComponent<IGameManager>.Unregister(this);
    }

    private void Start()
    {
        _movementRuleManager.Initialize();
    }

    public void HandleFailure()
    {
        _gameEventTrigger.TriggerEventActions(GameEvent.Failure);
    }

    public void HandleSuccess()
    {
        _gameEventTrigger.TriggerEventActions(GameEvent.Success);
    }

    public void HandleSceneEnd()
    {
        _gameEventTrigger.TriggerEventActions(GameEvent.SceneEnd);
    }
}
