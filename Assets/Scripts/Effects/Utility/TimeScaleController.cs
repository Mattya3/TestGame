using UnityEngine;

public class TimeScaleController : MonoEventReactingBehaviour
{
    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    protected override void OnSceneEnd()
    {
        SetTimeScale(1f); // Reset time scale to normal when the scene ends
    }
}
