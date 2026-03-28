using System.Collections.Generic;
using UnityEngine;

public class CameraTargetFactory
{
    private IReadOnlyList<Player> _players;

    public CameraTargetFactory(IReadOnlyList<Player> players)
    {
        _players = players;
    }

    public ICameraTarget Create(Constants.CameraTargetMode mode)
    {
        switch (mode)
        {
            case Constants.CameraTargetMode.Players:
                return new CameraTarget.Players(_players);

            default:
            Debug.LogError($"Unknown camera target mode: {mode}");
            throw new System.ArgumentException($"Unknown camera target mode: {mode}");
        }
    }
}
