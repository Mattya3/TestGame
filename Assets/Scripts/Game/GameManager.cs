using UnityEngine;
using static Constants;

public class GameManager : MonoBehaviour, IGameManager
{
    [SerializeField]
    private MovementRuleManager _movementRuleManager;

    private void Awake()
    {
        AccessComponent<IGameManager>.Register(this);
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
}
