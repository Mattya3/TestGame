using System.Reflection;
using UnityEngine;
using static Constants;

public abstract class MonoEventReactingBehaviour : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        RegisterEventActions();
    }

    protected virtual void OnSuccess() { }

    protected virtual void OnFailure() { }

    protected virtual void OnSceneEnd() { }

    // オーバーライドするが，イベントを登録したくない場合はfalseを返すように実装
    protected virtual bool _ShouldSubscribe(GameEvent gameEvent) => true;

    protected void RegisterEventActions()
    {
        if (_IsOverridden(nameof(OnSuccess)) && _ShouldSubscribe(GameEvent.Success))
        {
            GameEventTrigger.RegisterEventAction(GameEvent.Success, OnSuccess);
        }
        if (_IsOverridden(nameof(OnFailure)) && _ShouldSubscribe(GameEvent.Failure))
        {
            GameEventTrigger.RegisterEventAction(GameEvent.Failure, OnFailure);
        }
        if (_IsOverridden(nameof(OnSceneEnd)) && _ShouldSubscribe(GameEvent.SceneEnd))
        {
            GameEventTrigger.RegisterEventAction(GameEvent.SceneEnd, OnSceneEnd);
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
