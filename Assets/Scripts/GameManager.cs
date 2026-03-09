using UnityEngine;
using UnityEngine.SceneManagement;
using static GameConstants;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsPlayerAlive { get; private set; } = true;

    [SerializeField]
    private UIEffectController _uiEffectController;

    [SerializeField]
    private float _deathYThreshold;

    [SerializeField]
    private Player _player;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Update()
    {
        if (!IsPlayerAlive || _player == null) return;

        if (_player.transform.position.y < _deathYThreshold)
        {
            HandlePlayerDeath(DeathReason.Fall);
        }
    }

    /// <summary>
    /// プレイヤーが死亡したときに呼び出されます。
    /// </summary>
    public void HandlePlayerDeath(DeathReason reason)
    {
        IsPlayerAlive = false;
        _player.OnFreeze();

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
