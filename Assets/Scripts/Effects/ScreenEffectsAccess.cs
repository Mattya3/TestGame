using UnityEngine;

public class ScreenEffectsAccess : MonoBehaviour
{
    private static IScreenEffects _reference;

    public static void Register(IScreenEffects reference)
    {
        _reference = reference;
    }

    public static void Unregister(IScreenEffects reference)
    {
        if (_reference != reference)
            return;

        _reference = null;
    }

    private static ScreenEffectsAccess _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError(
                "Multiple instances of ScreenEffectsAccess detected. This is not allowed."
            );
            return;
        }
        _instance = this;
    }

    private void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }

    public void PlayOpeningEffect(System.Action onComplete)
    {
        _reference?.PlayOpeningEffect(onComplete);
    }

    public void PlayRestartEffect(System.Action onComplete)
    {
        _reference?.PlayRestartEffect(onComplete);
    }

    public void PlayFailureEffect(System.Action onComplete)
    {
        _reference?.PlayFailureEffect(onComplete);
    }

    public void PlaySuccessEffect(System.Action onComplete)
    {
        _reference?.PlaySuccessEffect(onComplete);
    }
}
