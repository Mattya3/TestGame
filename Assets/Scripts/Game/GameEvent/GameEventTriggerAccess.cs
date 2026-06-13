using UnityEngine;

public class GameEventTriggerAccess : AccessComponent<GameEventHub>
{
    private GameEventTriggerAccess _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("Multiple instances of GameEventTriggerAccess detected. This is not allowed.");
            return;
        }
        _instance = this;
    }

    private void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }

    public void TriggerEventActions(Constants.GameEvent gameEvent)
    {
        Reference?.TriggerEventActions(gameEvent);
    }
}
