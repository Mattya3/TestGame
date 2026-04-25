using System.Reflection;
using UnityEngine;
using static Constants;

[RequireComponent(typeof(GameEventRegistrationAccess))]
public abstract class MonoEventReactingBehaviour : MonoBehaviour
{
    private GameEventRegistrationAccess _eventRegistration;

    protected virtual void Awake()
    {
        _eventRegistration = GetComponent<GameEventRegistrationAccess>();
    }

    protected virtual void OnEnable()
    {
        if (_IsOverridden(nameof(OnSuccess)) && _ShouldSubscribe(GameEvent.Success))
        {
            _eventRegistration.RegisterEventAction(GameEvent.Success, OnSuccess);
        }

        if (_IsOverridden(nameof(OnFailure)) && _ShouldSubscribe(GameEvent.Failure))
        {
            _eventRegistration.RegisterEventAction(GameEvent.Failure, OnFailure);
        }
    }

    protected virtual void OnDisable()
    {
        if (_IsOverridden(nameof(OnSuccess)) && _ShouldSubscribe(GameEvent.Success))
        {
            _eventRegistration.UnregisterEventAction(GameEvent.Success, OnSuccess);
        }
        if (_IsOverridden(nameof(OnFailure)) && _ShouldSubscribe(GameEvent.Failure))
        {
            _eventRegistration.UnregisterEventAction(GameEvent.Failure, OnFailure);
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
