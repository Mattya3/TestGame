using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Constants;

public class GameEventTrigger : MonoBehaviour
{
    [Serializable]
    public class EventReaction
    {
        public GameEvent TargetEvent; // イベントの種類
        public UnityEvent OnEventTriggered; // 発火時のアクション
    }

    [SerializeField]
    private List<EventReaction> _reactions = new List<EventReaction>();

    private void Start()
    {
        foreach (var reaction in _reactions)
        {
            GameManager.Instance.RegisterEventAction(
                reaction.TargetEvent,
                () => reaction.OnEventTriggered?.Invoke()
            );
        }
    }
}
