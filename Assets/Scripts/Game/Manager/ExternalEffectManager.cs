using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class ExternalEffectManager : MonoBehaviour
{
    [SerializeField]
    private ExternalEffectType _externalEffectType;

    private IReadOnlyList<Player> _players;
    private bool _isDualInputActive;

    public void Initialize(IReadOnlyList<Player> players)
    {
        _players = players;
        foreach (var player in players)
        {
            player.OnInputDirectionChanged += _HandleDualInputChanged;
        }
        _UpdateDualInputState();
    }

    private void _HandleDualInputChanged(Player _, Vector2 __)
    {
        _UpdateDualInputState();
    }

    private void _UpdateDualInputState()
    {
        bool nextState = _HasDualInput();
        if (nextState == _isDualInputActive)
            return;

        _isDualInputActive = nextState;
        _ApplyExternalEffect(_isDualInputActive);
    }

    private bool _HasDualInput()
    {
        return _players[0].InputDirection.x != 0f
            && _players[1].InputDirection.x != 0f;
    }

    private void _ApplyExternalEffect(bool isDualInputActive)
    {
        for (int i = 0; i < _players.Count; i++)
        {
            Player player = _players[i];

            if (!isDualInputActive)
            {
                player.ResetExternalEffectBehavior();
                continue;
            }

            player.ApplyExternalEffectBehavior(_CreateExternalEffectBehavior(player));
        }
    }

    private Player.EffectBehavior _CreateExternalEffectBehavior(Player player)
    {
        switch (_externalEffectType)
        {
            case ExternalEffectType.ReverseInput:
                return new Player.ReverseInputBehavior(player);
            case ExternalEffectType.ReverseGravity:
                return new Player.ReverseGravityBehavior(player);
            default:
                return new Player.EffectBehavior(player);
        }
    }
}
