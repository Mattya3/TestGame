using System;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class GameEventHub : MonoBehaviour
{
    private readonly Dictionary<GameEvent, Action> _eventActions = new Dictionary<GameEvent, Action>
    {
        { GameEvent.Failure, null },
        { GameEvent.Success, null }
    };

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
        if (!_eventActions.ContainsKey(gameEvent))
        {
            Debug.LogError($"Unhandled GameEvent value in RegisterEventAction: {gameEvent}");
            throw new ArgumentOutOfRangeException(nameof(gameEvent), gameEvent, null);
        }
        
        _eventActions[gameEvent] += eventAction;
    }

    public void UnregisterEventAction(GameEvent gameEvent, Action eventAction)
    {
        if (!_eventActions.ContainsKey(gameEvent))
        {
            Debug.LogError($"Unhandled GameEvent value in UnregisterEventAction: {gameEvent}");
            throw new ArgumentOutOfRangeException(nameof(gameEvent), gameEvent, null);
        }
        
        _eventActions[gameEvent] -= eventAction;
    }

    public void TriggerEventActions(GameEvent gameEvent)
    {
        if (_eventActions.TryGetValue(gameEvent, out var action))
        {
            action?.Invoke();
        }
    }
}
