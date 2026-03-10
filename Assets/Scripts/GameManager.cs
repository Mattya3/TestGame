using UnityEngine;
using UnityEngine.SceneManagement;
using static GameConstants;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsPlayerAlive { get; private set; } = true;

    [SerializeField]
    private UIEffectController _uiEffectController;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    /// <summary>
    /// プレイヤーが死亡したときに呼び出されます。
    /// </summary>
    public void HandlePlayerDeath(Player deadPlayer, DeathReason deathReason)
    {
        if (!IsPlayerAlive) return;
        IsPlayerAlive = false;
        
        Player[] allPlayers = Object.FindObjectsByType<Player>(FindObjectsSortMode.None);
        foreach (Player p in allPlayers)
        {
            p.Freeze();
        }
        _RestartStage();
    }

    private void _RestartStage()
    {
        _uiEffectController.PlayDeathEffect(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }
}
