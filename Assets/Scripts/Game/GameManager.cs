using UnityEngine;
using static Constants;

[RequireComponent(typeof(GameEventTriggerAccess))]
[RequireComponent(typeof(StageSceneContextAccess))]
[RequireComponent(typeof(ScreenEffectsAccess))]
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private SceneTransitionManager _sceneTransitionManager;

    [SerializeField]
    private MovementRuleManager _movementRuleManager;

    [SerializeField]
    private PlayersManager _playersManager;

    private GameEventTriggerAccess _gameEventTrigger;
    private StageSceneContextAccess _stageSceneContext;
    private ScreenEffectsAccess _screenEffects;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _gameEventTrigger = GetComponent<GameEventTriggerAccess>();
        _stageSceneContext = GetComponent<StageSceneContextAccess>();
        _screenEffects = GetComponent<ScreenEffectsAccess>();
    }

    private void Start()
    {
        _movementRuleManager.Initialize();

        if (_stageSceneContext.AfterRestart)
            _screenEffects.PlayRestartEffect(() => { });
        else
            _screenEffects.PlayOpeningEffect(() => { });
    }

    public void HandleFailure()
    {
        _gameEventTrigger.TriggerEventActions(GameEvent.Failure);
        _stageSceneContext.OnStageRestarted();
        _screenEffects.PlayFailureEffect(() => _sceneTransitionManager.RestartStage());
    }

    public void HandleSuccess()
    {
        _gameEventTrigger.TriggerEventActions(GameEvent.Success);
        _screenEffects.PlaySuccessEffect(() => _sceneTransitionManager.CompleteStage());
    }
}
