using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class ExternalEffectManager : MonoBehaviour
{
    [SerializeField]
    private ExternalEffectType _externalEffectType;

    private IReadOnlyList<Player> _players;
    private DualInputTrigger _trigger;

    public void Initialize(IReadOnlyList<Player> players)
    {
        _players = players;
        _trigger = new DualInputTrigger(players);

        _trigger.OnDualInputStateChanged += ApplyEffectToPlayers;
    }

    private void ApplyEffectToPlayers(bool isActive)
    {
        for (int i = 0; i < _players.Count; i++)
        {
            Player player = _players[i];

            if (!isActive)
            {
                player.ApplyExternalEffectType(ExternalEffectType.None);
            }
            else
            {
                player.ApplyExternalEffectType(_externalEffectType);
            }
        }
    }
}
