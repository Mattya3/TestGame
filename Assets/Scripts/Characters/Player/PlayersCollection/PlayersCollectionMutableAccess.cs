using UnityEngine;

public class PlayersCollectionMutableAccess : MonoBehaviour
{
    private static IPlayersCollection _reference;

    public static void Register(IPlayersCollection reference)
    {
        _reference = reference;
    }

    public static void Unregister(IPlayersCollection reference)
    {
        if (_reference != reference)
            return;

        _reference = null;
    }

    public void RegisterPlayer(Player player)
    {
        _LogMissingReference();
        _reference?.RegisterPlayer(player);
    }

    public void SetMoveController(IMoveController moveController)
    {
        _LogMissingReference();
        _reference?.SetMoveController(moveController);
    }

    private void _LogMissingReference()
    {
        if (_reference == null)
        {
            Debug.LogError(
                "No IPlayersCollection reference registered. Please ensure that an IPlayersCollection implementation is registered before using PlayersCollectionMutableAccess."
            );
        }
    }
}
