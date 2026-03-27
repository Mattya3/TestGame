using UnityEngine;
using static Constants;

public class DeadZoneTrigger : MonoBehaviour
{
    [SerializeField]
    private DeathReason _deathReason;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            player.Die(_deathReason);
        }
    }
}
