using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Constants;

[RequireComponent(typeof(ScreenEffectsAccess))]
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private SceneTransitionManager _sceneTransitionManager;

    [SerializeField]
    private MovementRuleManager _movementRuleManager;

    [SerializeField]
    private PlayersManager _playersManager;

    private ScreenEffectsAccess _screenEffects;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _screenEffects = GetComponent<ScreenEffectsAccess>();
    }

    private void Start()
    {
        _movementRuleManager.Initialize(_playersManager.Players);

        _screenEffects?.PlayOpeningEffect(() => { });
    }

    public void HandleFailure()
    {
        GameEventTrigger.TriggerEvent(GameEvent.Failure);
        GameEventTrigger.ResetEvents();
        _screenEffects?.PlayFailureEffect(() => _sceneTransitionManager.RestartStage());
    }

    public void HandleSuccess()
    {
        GameEventTrigger.TriggerEvent(GameEvent.Success);
        GameEventTrigger.ResetEvents();
        _screenEffects?.PlaySuccessEffect(() => _sceneTransitionManager.CompleteStage());
    }

    public IReadOnlyList<Player> Players => _playersManager.Players;
}
