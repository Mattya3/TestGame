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
        IReadOnlyList<Player> players = _playersManager.Players;
        List<IExternalEffectContext> contexts = new(players.Count);
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] is IExternalEffectContext context)
            {
                contexts.Add(context);
            }
        }
        _externalEffectManager.Initialize(players, contexts);
    }

    public void HandleFailure()
    {
        GameEventTrigger.TriggerEvent(GameEvent.Failure);
        GameEventTrigger.ResetEvents();
        _sceneTransitionManager.RestartStage();
    }

    public void HandleSuccess()
    {
        GameEventTrigger.TriggerEvent(GameEvent.Success);
        GameEventTrigger.ResetEvents();
        _sceneTransitionManager.CompleteStage();
    }

    public IReadOnlyList<Player> Players => _playersManager.Players;
}
