using UnityEngine;

public class StageSceneContextAccess : MonoBehaviour
{
    private static IStageSceneContext _reference;

    public static void Register(IStageSceneContext reference)
    {
        _reference = reference;
    }

    public static void Unregister(IStageSceneContext reference)
    {
        if (_reference != reference)
            return;

        _reference = null;
    }

    private static StageSceneContextAccess _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("Multiple instances of StageSceneContextAccess detected. This is not allowed.");
            return;
        }
        _instance = this;
    }

    private void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }

    public bool AfterRestart => _reference != null ? _reference.AfterRestart : false;

    public void OnStageRestarted()
    {
        _reference?.OnStageRestarted();
    }
}
