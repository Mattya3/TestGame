using UnityEngine;

public class StageSceneContext : MonoBehaviour, IStageSceneContext
{
    [SerializeField]
    private uint _restartCount = 0;

    private static StageSceneContext _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        StageSceneContextAccess.Register(this);
    }

    private void OnDestroy()
    {
        StageSceneContextAccess.Unregister(this);
    }

    public bool AfterRestart => _restartCount > 0;

    public void OnStageRestarted()
    {
        _restartCount++;
    }
}
