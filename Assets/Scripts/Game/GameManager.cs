using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Constants;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private SceneTransitionManager _sceneTransitionManager;

    [SerializeField]
    private MovementRuleManager _movementRuleManager;

    [SerializeField]
    private PlayersManager _playersManager;

    [SerializeField]
    private GameObject _screenEffectsAccessPrefab;

    private ScreenEffectsAccess _screenEffects;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (_screenEffectsAccessPrefab == null)
        {
            Debug.LogError("ScreenEffectsAccess Prefab is not assigned.", this);
            enabled = false;
            return;
        }
        _screenEffects = Instantiate(_screenEffectsAccessPrefab).GetComponent<ScreenEffectsAccess>();
        if (_screenEffects == null)
        {
            Debug.LogError("ScreenEffectsAccess component is missing in the prefab.", this);
            enabled = false;
            return;
        }
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
