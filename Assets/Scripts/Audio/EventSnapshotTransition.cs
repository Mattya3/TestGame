using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static Constants;

public class EventSnapshotTransition : MonoEventReactingBehaviour
{
    [Serializable]
    private struct SnapshotTransition
    {
        [SerializeField]
        private GameEvent _gameEvent;
        [SerializeField]
        private AudioMixerSnapshot _snapshot;
        [SerializeField]
        private float _transitionTime;

        public void TryTransition(GameEvent gameEvent)
        {
            if (gameEvent == _gameEvent)
            {
                _snapshot.TransitionTo(_transitionTime);
            }
        }
    }

    [SerializeField]
    private List<SnapshotTransition> _transitions;

    protected override void OnGameEvent(GameEvent gameEvent)
    {
        foreach (var transition in _transitions)
        {
            transition.TryTransition(gameEvent);
        }
    }
}
