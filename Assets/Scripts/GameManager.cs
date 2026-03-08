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
        _uiEffectController.PlayDeathEffect(
            () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        );
    }
}
