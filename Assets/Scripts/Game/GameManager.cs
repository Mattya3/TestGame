using UnityEngine;
using static Constants;

[RequireComponent(typeof(GameEventTriggerAccess))]
[RequireComponent(typeof(StageSceneContextAccess))]
[RequireComponent(typeof(ScreenEffectsAccess))]
[RequireComponent(typeof(CameraAccess))]
public class GameManager : MonoBehaviour, IGameManager
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
    private CameraAccess _camera;

    private void Awake()
    {
        GameManagerAccess.Register(this);

        _gameEventTrigger = GetComponent<GameEventTriggerAccess>();
        _stageSceneContext = GetComponent<StageSceneContextAccess>();
        _screenEffects = GetComponent<ScreenEffectsAccess>();
        _camera = GetComponent<CameraAccess>();
    }

    private void OnDestroy()
    {
        GameManagerAccess.Unregister(this);
    }

    private void Start()
    {
        _movementRuleManager.Initialize();

        if (_stageSceneContext.AfterRestart)
            _screenEffects.PlayRestartEffect(() => _camera.PopTarget());
        else
            _screenEffects.PlayOpeningEffect(() => _camera.PopTarget());
    }

    public void OnFailure()
    {
        _gameEventTrigger.TriggerEventActions(GameEvent.Failure);
        _stageSceneContext.OnStageRestarted();
        _screenEffects.PlayFailureEffect(() => _sceneTransitionManager.RestartStage());
    }

    public void OnSuccess()
    {
        _gameEventTrigger.TriggerEventActions(GameEvent.Success);
        _screenEffects.PlaySuccessEffect(() => _sceneTransitionManager.CompleteStage());
    }
}
