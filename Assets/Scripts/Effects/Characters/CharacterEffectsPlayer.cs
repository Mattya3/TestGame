using UnityEngine;

public class CharacterEffectsPlayer : MonoBehaviour
{
    [Header("Reference")]

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private CameraMutableAccess _cameraAccess;

    [SerializeField]
    private TransformOffsetController _transformOffsetController;

    [SerializeField]
    private Renderer _renderer;

    [Header("Effect Prefabs")]

    [SerializeField]
    private GameObject _deadZoneDeathEffectPrefab;

    [SerializeField]
    private GameObject _fallDeathEffectPrefab;

    private EffectsCompositePlayer _deadZonePlayer;
    private EffectsCompositePlayer _fallPlayer;

    private void Awake()
    {
        _deadZonePlayer = new EffectsCompositePlayer(_deadZoneDeathEffectPrefab, _audioSource, _cameraAccess, _transformOffsetController, _renderer, transform.position, transform.rotation, transform);
        _fallPlayer = new EffectsCompositePlayer(_fallDeathEffectPrefab, _audioSource, _cameraAccess, _transformOffsetController, _renderer, transform.position, transform.rotation, transform);
    }

    private void OnDestroy()
    {
        _deadZonePlayer?.Cleanup();
        _fallPlayer?.Cleanup();
    }

    public void PlayDeathEffect(Constants.DeathReason deathReason)
    {
        EffectsCompositePlayer player = null;
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

        player?.PlayEffects();
    }
}
