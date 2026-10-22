using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalEffectManager : MonoBehaviour
{
    [SerializeField]
    private Constants.ExternalEffectType _player1ExternalEffectType = Constants
        .ExternalEffectType
        .None;

    [SerializeField]
    private Constants.ExternalEffectType _player2ExternalEffectType = Constants
        .ExternalEffectType
        .None;

    private IEnumerator Start()
    {
        Player[] players = null;
        yield return new WaitUntil(() =>
        {
            players = FindObjectsByType<Player>(FindObjectsSortMode.InstanceID);
            return players.Length == Constants.PLAYER_COUNT;
        });
        Initialize(players);
    }

    private void Initialize(IReadOnlyList<Player> players)
    {
        if (players == null)
        {
            Debug.LogError("players が null です。", this);
            return;
        }

        for (int i = 0; i < players.Count; i++)
        {
            Player player = players[i];
            if (player == null)
                continue;

            Constants.ExternalEffectType effectType = _GetExternalEffectType(i);
            IExternalEffect externalEffect = ExternalEffectFactory.Create(effectType, players, i);
            if (player.ExternalEffectApplier == null)
            {
                Debug.LogError("PlayerExternalEffectApplier が初期化されていません。", player);
                continue;
            }
            player.ExternalEffectApplier.SetExternalEffect(externalEffect);
        }
    }

    private Constants.ExternalEffectType _GetExternalEffectType(int playerIndex)
    {
        if (playerIndex == 0)
            return _player1ExternalEffectType;
        if (playerIndex == 1)
            return _player2ExternalEffectType;

        return Constants.ExternalEffectType.None;
    }
}
