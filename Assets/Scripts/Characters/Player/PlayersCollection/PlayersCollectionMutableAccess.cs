using UnityEngine;

public class PlayersCollectionMutableAccess : AccessComponent<IPlayersCollection>
{
    public void RegisterPlayer(Player player)
    {
        Reference?.RegisterPlayer(player);
    }

    public void SetMoveController(IMoveController moveController)
    {
        Reference?.SetMoveController(moveController);
    }
}
