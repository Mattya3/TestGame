using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CharacterEffectsPlayer : MonoBehaviour
{

    [SerializeField]
    private VisualEffectsPool.EffectConfig _deadZoneDeathEffect;

    [SerializeField]
    private VisualEffectsPool.EffectConfig _fallDeathEffect;

    private VisualEffectsPool.IEffectPlayer _deadZonePlayer;
    private VisualEffectsPool.IEffectPlayer _fallPlayer;

    private void Awake()
    {
        _deadZonePlayer = _deadZoneDeathEffect.CreateEffectPlayer(transform);
        _fallPlayer = _fallDeathEffect.CreateEffectPlayer(transform);
    }

    private void OnDestroy()
    {
        _deadZonePlayer?.Cleanup(this);
        _fallPlayer?.Cleanup(this);
    }

    public void PlayDeathEffect(Constants.DeathReason deathReason)
    {
        VisualEffectsPool.IEffectPlayer player = null;
        switch (deathReason)
        {
            case Constants.DeathReason.DeadZone:
                player = _deadZonePlayer;
                break;
            case Constants.DeathReason.Fall:
                player = _fallPlayer;
                break;
            default:
                Debug.LogError($"Unhandled DeathReason value: {deathReason}");
                return;
        }

        player?.Play(this);
    }
}
