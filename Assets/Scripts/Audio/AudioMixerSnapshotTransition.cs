using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static Constants;

public class AudioMixerSnapshotTransition : MonoEventReactingBehaviour
{
    [Serializable]
    private struct EventTransition
    {
        [SerializeField]
        private GameEvent _gameEvent;
        [SerializeField]
        private AudioMixerSnapshot _snapshot;
        [SerializeField, Min(0f)]
        private float _transitionTime;

        public void TryTransition(GameEvent gameEvent)
        {
            if (gameEvent == _gameEvent)
            {
                if (_snapshot == null)
                {
                    Debug.LogError("Snapshot is not assigned for the transition.");
                    return;
                }
                _snapshot.TransitionTo(_transitionTime);
            }
        }
    }

    [SerializeField]
    private AudioMixerSnapshot _startupSnapshot;

    [SerializeField]
    [UnityEngine.Serialization.FormerlySerializedAs("_transitions")]
    private List<EventTransition> _eventTransitions;
    
    private void Awake()
    {
        if (_startupSnapshot == null)
        {
            Debug.LogError("Startup snapshot is not assigned.");
            enabled = false;
            return;
        }
        _startupSnapshot.TransitionTo(0f);
    }

    protected override void OnGameEvent(GameEvent gameEvent)
    {
        foreach (var transition in _eventTransitions)
        {
            transition.TryTransition(gameEvent);
        }
    }
}
