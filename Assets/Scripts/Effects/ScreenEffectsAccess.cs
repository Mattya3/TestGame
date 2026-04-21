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
