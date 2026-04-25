using System;
using UnityEngine;
using static Constants;

public class GameEventHub : MonoBehaviour
{
    private event Action _onSuccess;
    private event Action _onFailure;

    private void Awake()
    {
        GameEventRegistrationAccess.Register(this);
        GameEventTriggerAccess.Register(this);
    }

    private void OnDestroy()
    {
        GameEventTriggerAccess.Unregister(this);
        GameEventRegistrationAccess.Unregister(this);
    }

    public void RegisterEventAction(GameEvent gameEvent, Action eventAction)
    {
        switch (gameEvent)
        {
            case GameEvent.Failure:
                _onFailure += eventAction;
                break;
            case GameEvent.Success:
                _onSuccess += eventAction;
                break;
            default:
                Debug.LogError($"Unhandled GameEvent value in RegisterEventAction: {gameEvent}");
                throw new ArgumentOutOfRangeException(nameof(gameEvent), gameEvent, null);
        }
    }

    public void TriggerEventActions(GameEvent gameEvent)
    {
        if (gameEvent == GameEvent.Success)
            _onSuccess?.Invoke();
        else if (gameEvent == GameEvent.Failure)
            _onFailure?.Invoke();
    }
}
