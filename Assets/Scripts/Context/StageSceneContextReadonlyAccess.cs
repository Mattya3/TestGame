using UnityEngine;

public class StageSceneContextReadonlyAccess : AccessComponent<IStageSceneContext>
{
    public bool AfterRestart => Reference != null && Reference.AfterRestart;
}
