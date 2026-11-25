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
        // シーン終了時、登録先のGameEventHubが既に破棄されている可能性があるため、参照の有無を確認してから処理を行う
        if (!HasReference)
            return;

        Reference?.UnregisterEventAction(gameEvent, eventAction);
    }

    public void RegisterArgumentedEventAction(Action<GameEvent> eventAction)
    {
        Reference?.RegisterArgumentedEventAction(eventAction);
    }

    public void UnregisterArgumentedEventAction(Action<GameEvent> eventAction)
    {
        if (!HasReference)
            return;

        Reference?.UnregisterArgumentedEventAction(eventAction);
    }
}
