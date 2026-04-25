using UnityEngine;

public class GameManagerAccess : MonoBehaviour
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

    public void OnFailure()
    {
        _reference?.OnFailure();
    }

    public void OnSuccess()
    {
        _reference?.OnSuccess();
    }
}
