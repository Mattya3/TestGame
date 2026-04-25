using UnityEngine;
using static Constants;

[RequireComponent(typeof(StageSceneContextAccess))]
[RequireComponent(typeof(ScreenEffectsAccess))]
public class GameManager : MonoBehaviour, IGameManager
{
    [SerializeField]
    private SceneTransitionManager _sceneTransitionManager;

    [SerializeField]
    private MovementRuleManager _movementRuleManager;

    [SerializeField]
    private PlayersManager _playersManager;

    private StageSceneContextAccess _stageSceneContext;
    private ScreenEffectsAccess _screenEffects;

    private void Awake()
    {
        GameManagerAccess.Register(this);

        _stageSceneContext = GetComponent<StageSceneContextAccess>();
        _screenEffects = GetComponent<ScreenEffectsAccess>();
    }

    private void OnDestroy()
    {
        GameManagerAccess.Unregister(this);
    }

    private void Start()
    {
        _movementRuleManager.Initialize();

        if (_stageSceneContext.AfterRestart)
            _screenEffects.PlayRestartEffect(() => { });
        else
            _screenEffects.PlayOpeningEffect(() => { });
    }

    public void OnFailure()
    {
        GameEventTrigger.TriggerEvent(GameEvent.Failure);
        GameEventTrigger.ResetEvents();
        _stageSceneContext.OnStageRestarted();
        _screenEffects.PlayFailureEffect(() => _sceneTransitionManager.RestartStage());
    }

    public void OnSuccess()
    {
        GameEventTrigger.TriggerEvent(GameEvent.Success);
        GameEventTrigger.ResetEvents();
        _screenEffects.PlaySuccessEffect(() => _sceneTransitionManager.CompleteStage());
    }
}
