using System;
using UnityEngine;
using static Constants;

public class GameEventRegistrationAccess : AccessComponent<GameEventHub>
{
    public void RegisterEventAction(GameEvent gameEvent, Action eventAction)
    {
        Reference?.RegisterEventAction(gameEvent, eventAction);
    }

    public void UnregisterEventAction(GameEvent gameEvent, Action eventAction)
    {
        Reference?.UnregisterEventAction(gameEvent, eventAction);
    }
}
