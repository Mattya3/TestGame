using System.Collections.Generic;
using UnityEngine;

public static class ExternalEffectCondition
{
    public static bool AreAllPlayersInputtingHorizontal(PlayersCollectionReadonlyAccess playersAccess)
    {
        if (playersAccess == null)
            return false;

        var inputDirections = playersAccess.InputDirections;
        if (inputDirections == null || inputDirections.Count != playersAccess.Count)
            return false;

        for (int i = 0; i < playersAccess.Count; i++)
        {
            if (Mathf.Approximately(inputDirections[i].x, 0f))
                return false;
        }

        return true;
    }
}
