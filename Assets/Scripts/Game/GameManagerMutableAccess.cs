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
        _LogMissingReference();
        _reference?.HandleFailure();
    }

    public void HandleSuccess()
    {
        _LogMissingReference();
        _reference?.HandleSuccess();
    }

    public void HandleSceneEnd()
    {
        _LogMissingReference();
        _reference?.HandleSceneEnd();
    }

    private void _LogMissingReference()
    {
        if (_reference == null)
        {
            Debug.LogError(
                "No IGameManager reference registered. Please ensure that an IGameManager implementation is registered before using GameManagerMutableAccess."
            );
        }
    }
}
