using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoEventReactingBehaviour
{
    private enum TransitionMode
    {
        Restart,
        Success,
    }

    private TransitionMode _transitionMode = TransitionMode.Restart;

    protected override void OnSuccess()
    {
        _transitionMode = TransitionMode.Success;
    }

    protected override void OnFailure()
    {
        _transitionMode = TransitionMode.Restart;
    }

    protected override void OnSceneEnd()
    {
        if (_transitionMode == TransitionMode.Restart)
            _RestartStage();
        else if (_transitionMode == TransitionMode.Success)
            _CompleteStage();
    }

    public void _RestartStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void _CompleteStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
