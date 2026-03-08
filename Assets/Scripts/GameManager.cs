using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private UIEffectController _uiEffectController;

    /// <summary>
    /// プレイヤーが死亡したときに呼び出されます。
    /// </summary>
    public void HandlePlayerDeath()
    {
        _RestartStage();
    }

    private void _RestartStage()
    {
        _uiEffectController.PlayEffect(
            GameConstants.UI.FADE_BLACK_OUT,
            () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        );
    }
}
