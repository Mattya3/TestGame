using UnityEngine;

public class StageSceneContextReadonlyAccess : MonoBehaviour
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

    public bool AfterRestart
    {
        get
        {
            _LogMissingReference();
            return _reference != null ? _reference.AfterRestart : false;
        }
    }

    private void _LogMissingReference()
    {
        if (_reference == null)
        {
            Debug.LogError(
                "No IStageSceneContext reference registered. Please ensure that an IStageSceneContext implementation is registered before using StageSceneContextAccess."
            );
        }
    }
}
