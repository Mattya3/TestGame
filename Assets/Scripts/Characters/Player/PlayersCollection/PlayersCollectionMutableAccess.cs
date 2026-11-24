using UnityEngine;

public class PlayersCollectionMutableAccess : AccessComponent<IPlayersCollection>
{
    public int Count => Reference != null ? Reference.Count : 0;
    public IExternalEffectTarget CreatePlayerExternalEffectTarget(int index) => Reference != null ? Reference.CreatePlayerExternalEffectTarget(index) : null;
}
