﻿using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[RequireComponent(typeof(GameEventRegistrationAccess))]
public abstract class MonoEventReactingBehaviour : MonoBehaviour
{
    private GameEventRegistrationAccess _eventRegistrationAccess;

    private readonly Dictionary<GameEvent, (string MethodName, Action Handler)> _eventHandlers;

    protected MonoEventReactingBehaviour()
    {
        _eventHandlers = new Dictionary<GameEvent, (string, Action)>
        {
            { GameEvent.Success, (nameof(OnSuccess), OnSuccess) },
            { GameEvent.Failure, (nameof(OnFailure), OnFailure) },
            { GameEvent.SceneEnd, (nameof(OnSceneEnd), OnSceneEnd) },
        };
    }

    protected virtual void Awake()
    {
        _eventRegistrationAccess = GetComponent<GameEventRegistrationAccess>();
    }

    protected virtual void OnEnable()
    {
        RegisterEventActions();
    }

    protected virtual void OnDisable()
    {
        UnregisterEventActions();
    }

    protected virtual void OnSuccess() { }

    protected virtual void OnFailure() { }

    protected virtual void OnSceneEnd() { }

    // オーバーライドするが，イベントを登録したくない場合はfalseを返すように実装
    protected virtual bool _ShouldSubscribe(GameEvent gameEvent) => true;

    protected void RegisterEventActions()
    {
        foreach (var kvp in _eventHandlers)
        {
            var gameEvent = kvp.Key;
            var (methodName, handler) = kvp.Value;
            if (_IsOverridden(methodName) && _ShouldSubscribe(gameEvent))
                _eventRegistrationAccess.RegisterEventAction(gameEvent, handler);
        }
    }

    protected void UnregisterEventActions()
    {
        foreach (var kvp in _eventHandlers)
        {
            var gameEvent = kvp.Key;
            var (methodName, handler) = kvp.Value;
            if (_IsOverridden(methodName) && _ShouldSubscribe(gameEvent))
                _eventRegistrationAccess.UnregisterEventAction(gameEvent, handler);
        }
    }

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
