using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private FadeController _fadeController;

    /// <summary>
    /// プレイヤーが死亡したときに呼び出されます。
    /// </summary>
    public void HandlePlayerDeath()
    {
        StartCoroutine(_RestartStage());
    }

    private IEnumerator _RestartStage()
    {
        if (_fadeController != null)
        {
            yield return _fadeController.FadeOut();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
