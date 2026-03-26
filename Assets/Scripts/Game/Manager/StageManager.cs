using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    private ScreenEffectsController _screenEffectsController;

    public void RestartStage()
    {
        _screenEffectsController.PlayFailureEffect(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }

    public void CompleteStage()
    {
        _screenEffectsController.PlaySuccessEffect(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }
}
