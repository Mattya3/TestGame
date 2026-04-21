using System;

public interface IScreenEffects
{
    public void PlayOpeningEffect(Action onComplete);
    public void PlayRestartEffect(Action onComplete);
    public void PlayFailureEffect(Action onComplete);
    public void PlaySuccessEffect(Action onComplete);
}
