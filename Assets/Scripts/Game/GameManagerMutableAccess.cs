using UnityEngine;

public class GameManagerMutableAccess : AccessComponent<IGameManager>
{
    public void HandleFailure()
    {
        Reference?.HandleFailure();
    }

    public void HandleSuccess()
    {
        Reference?.HandleSuccess();
    }

    public void HandleSceneEnd()
    {
        Reference?.HandleSceneEnd();
    }
}
