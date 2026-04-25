using System;
using UnityEngine;
using static Constants;

public class GameEventRegistrationAccess : MonoBehaviour
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

    public void RegisterEventAction(GameEvent gameEvent, Action eventAction)
    {
        _gameEventHub?.RegisterEventAction(gameEvent, eventAction);
    }

    public void UnregisterEventAction(GameEvent gameEvent, Action eventAction)
    {
        _gameEventHub?.UnregisterEventAction(gameEvent, eventAction);
    }
}
