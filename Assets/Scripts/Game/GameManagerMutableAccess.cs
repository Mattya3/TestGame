using UnityEngine;

public class GameManagerMutableAccess : MonoBehaviour
{
    private static IGameManager _reference;

    public static void Register(IGameManager gameManager)
    {
        _reference = gameManager;
    }

    public static void Unregister(IGameManager gameManager)
    {
        if (_reference == gameManager)
            _reference = null;
    }

    public void HandleFailure()
    {
        _reference?.HandleFailure();
    }

    public void HandleSuccess()
    {
        _reference?.HandleSuccess();
    }

    public void HandleSceneEnd()
    {
        _reference?.HandleSceneEnd();
    }
}
