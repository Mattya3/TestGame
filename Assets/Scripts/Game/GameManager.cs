using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private SceneTransitionManager _sceneTransitionManager;

    [SerializeField]
    private ExternalEffectManager _externalEffectManager;

    [SerializeField]
    private PlayersManager _playersManager;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _externalEffectManager.Initialize(_playersManager.Players);
    }

    public void HandleFailure()
    {
        GameEventTrigger.TriggerEvent(Constants.GameEvent.Failure);
        GameEventTrigger.ResetEvents();
        _sceneTransitionManager.RestartStage();
    }

    public void HandleSuccess()
    {
        GameEventTrigger.TriggerEvent(Constants.GameEvent.Success);
        GameEventTrigger.ResetEvents();
        _sceneTransitionManager.CompleteStage();
    }

    public IReadOnlyList<Player> Players => _playersManager.Players;
}
