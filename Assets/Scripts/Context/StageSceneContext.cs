using UnityEngine;

public class StageSceneContext : MonoEventReactingBehaviour, IStageSceneContext
{
    [SerializeField]
    private uint _restartCount = 0;

    private static StageSceneContext _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            // GameEventTriggerに登録されたイベントアクションはシーン終了時にクリアされるため、シーン再読み込み後にイベントアクションを再登録する必要がある。
            _instance.RegisterEventActions();

            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        AccessComponent<IStageSceneContext>.Register(this);
    }

    private void OnDestroy()
    {
        AccessComponent<IStageSceneContext>.Unregister(this);
    }

    public bool AfterRestart => _restartCount > 0;

    protected override void OnFailure()
    {
        _restartCount++;
    }
}
