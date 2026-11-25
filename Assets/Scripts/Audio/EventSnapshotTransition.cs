using UnityEngine;
using UnityEngine.Audio;
using static Constants;

public class EventSnapshotTransition : MonoEventReactingBehaviour
{
    [SerializeField]
    private GameEvent _gameEvent;

    [SerializeField]
    private AudioMixerSnapshot _snapshot;

    [SerializeField]
    private float _transitionTime;

    protected override bool _ShouldSubscribe(GameEvent gameEvent)
    {
        return gameEvent == _gameEvent;
    }

    protected override void OnGameEvent(GameEvent gameEvent)
    {
        if (gameEvent == _gameEvent)
            _snapshot.TransitionTo(_transitionTime);
    }
}
