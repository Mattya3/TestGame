using UnityEngine;

public class ScreenEffectsAccess : MonoBehaviour
{
    static private IScreenEffects _reference;

    static public void Register(IScreenEffects reference)
    {
        _reference = reference;
    }

    static public void Unregister(IScreenEffects reference)
    {
        if (_reference != reference)
            return;

        _reference = null;
    }

    static private ScreenEffectsAccess _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("Multiple instances of ScreenEffectsAccess detected. This is not allowed.");
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
