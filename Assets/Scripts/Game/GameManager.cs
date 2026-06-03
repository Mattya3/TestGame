using UnityEngine;
using static Constants;

public class GameManager : MonoBehaviour, IGameManager
{
    [SerializeField]
    private MovementRuleManager _movementRuleManager;

    [SerializeField]
    private PlayersManager _playersManager;

    private void Awake()
    {
        GameManagerAccess.Register(this);
    }

    private void OnDestroy()
    {
        GameManagerAccess.Unregister(this);
    }

    private void Start()
    {
        _movementRuleManager.Initialize();
    }

    public void OnFailure()
    {
        GameEventTrigger.TriggerEvent(GameEvent.Failure);
    }

    public void OnSuccess()
    {
        GameEventTrigger.TriggerEvent(GameEvent.Success);
    }

    public void OnSceneEnd()
    {
        GameEventTrigger.TriggerEvent(GameEvent.SceneEnd);
        GameEventTrigger.ResetEvents();
    }
}
