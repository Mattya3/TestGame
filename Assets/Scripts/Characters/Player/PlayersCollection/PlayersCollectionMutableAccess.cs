using UnityEngine;

public class PlayersCollectionMutableAccess : AccessComponent<IPlayersCollection>
{
    public void SetMoveController(IMoveController moveController)
    {
        Reference?.SetMoveController(moveController);
    }
}
