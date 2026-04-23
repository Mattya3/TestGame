using UnityEngine;

public class PlayersCollectionAccess : MonoBehaviour
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
        _reference?.RegisterPlayer(player);
    }

    public void SetMoveController(IMoveController moveController)
    {
        _reference?.SetMoveController(moveController);
    }
}
