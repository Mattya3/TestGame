using UnityEngine;

public class GameEventTriggerAccess : MonoBehaviour
{
    private static GameEventHub _gameEventHub;

    public static void Register(GameEventHub gameEventHub)
    {
        _gameEventHub = gameEventHub;
    }

    public static void Unregister(GameEventHub gameEventHub)
    {
        if (_gameEventHub == gameEventHub)
            _gameEventHub = null;
    }

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
        _gameEventHub?.TriggerEventActions(gameEvent);
    }
}
