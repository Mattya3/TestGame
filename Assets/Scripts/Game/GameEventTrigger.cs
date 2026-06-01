using System;
using UnityEngine;
using static Constants;

public static class GameEventTrigger
{
    private static event Action OnSuccess;
    private static event Action OnFailure;
    private static event Action OnSceneEnd;

    public static void RegisterEventAction(GameEvent gameEvent, Action eventAction)
    {
        switch (gameEvent)
        {
            case GameEvent.Failure:
                OnFailure += eventAction;
                break;
            case GameEvent.Success:
                OnSuccess += eventAction;
                break;
            case GameEvent.SceneEnd:
                OnSceneEnd += eventAction;
                break;
            default:
                Debug.LogError($"Unhandled GameEvent value in RegisterEventAction: {gameEvent}");
                throw new ArgumentOutOfRangeException(nameof(gameEvent), gameEvent, null);
        }
    }

    public static void TriggerEvent(GameEvent gameEvent)
    {
        if (gameEvent == GameEvent.Success)
            OnSuccess?.Invoke();
        else if (gameEvent == GameEvent.Failure)
            OnFailure?.Invoke();
        else if (gameEvent == GameEvent.SceneEnd)
            OnSceneEnd?.Invoke();
    }

    public static void ResetEvents()
    {
        OnSuccess = null;
        OnFailure = null;
        OnSceneEnd = null;
    }
}
