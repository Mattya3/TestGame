using System.Collections.Generic;
using UnityEngine;

public static class CameraTargetFactory
{
    public static ICameraTarget Create(Constants.CameraTargetMode mode)
    {
        switch (mode)
        {
            case Constants.CameraTargetMode.Players:
                return new CameraTarget.Players(GameManager.Instance.Players);

            default:
                Debug.LogError($"Unknown camera target mode: {mode}");
                throw new System.ArgumentException($"Unknown camera target mode: {mode}");
        }
    }
}
