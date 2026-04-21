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

    public bool AfterRestart => _reference != null ? _reference.AfterRestart : false;

    public void OnStageRestarted()
    {
        _reference?.OnStageRestarted();
    }
}
