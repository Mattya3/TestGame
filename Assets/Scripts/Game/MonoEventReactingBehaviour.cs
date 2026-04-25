using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[RequireComponent(typeof(GameEventRegistrationAccess))]
public abstract class MonoEventReactingBehaviour : MonoBehaviour
{
    private GameEventRegistrationAccess _eventRegistration;

    private readonly Dictionary<GameEvent, (string MethodName, Action Handler)> _eventHandlers;

    protected MonoEventReactingBehaviour()
    {
        _eventHandlers = new Dictionary<GameEvent, (string, Action)>
        {
            { GameEvent.Success, (nameof(OnSuccess), OnSuccess) },
            { GameEvent.Failure, (nameof(OnFailure), OnFailure) }
        };
    }

    protected virtual void Awake()
    {
        _eventRegistration = GetComponent<GameEventRegistrationAccess>();
    }

    protected virtual void OnEnable()
    {
        foreach (var kvp in _eventHandlers)
        {
            var gameEvent = kvp.Key;
            var (methodName, handler) = kvp.Value;

            if (_IsOverridden(methodName) && _ShouldSubscribe(gameEvent))
                _eventRegistration.RegisterEventAction(gameEvent, handler);
        }
    }

    protected virtual void OnDisable()
    {
        foreach (var kvp in _eventHandlers)
        {
            var gameEvent = kvp.Key;
            var (methodName, handler) = kvp.Value;

            if (_IsOverridden(methodName) && _ShouldSubscribe(gameEvent))
                _eventRegistration.UnregisterEventAction(gameEvent, handler);
        }
    }

    protected virtual void OnSuccess() { }

    protected virtual void OnFailure() { }

    // オーバーライドするが，イベントを登録したくない場合はfalseを返すように実装
    protected virtual bool _ShouldSubscribe(GameEvent gameEvent) => true;

    private bool _IsOverridden(string methodName)
    {
        var method = GetType()
            .GetMethod(
                methodName,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
            );

        return method != null && method.DeclaringType != typeof(MonoEventReactingBehaviour);
    }
}
