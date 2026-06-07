using System.Collections.Generic;
using UnityEngine;

public static class ExternalEffectCondition
{
    public static bool AreAllPlayersInputtingHorizontal(IReadOnlyList<Player> players)
    {
        if (players == null || players.Count != 2)
            return false;

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == null)
                return false;
            if (Mathf.Approximately(players[i].InputDirection.x, 0f))
                return false;
        }

        return true;
    }
}
