using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CameraMutableAccess))]
[RequireComponent(typeof(TransformOffsetController))]
public class EffectsCompositeTester : MonoBehaviour
{
    [SerializeField]
    private GameObject _effectPrefab;

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private CameraMutableAccess _cameraAccess;

    [SerializeField]
    private TransformOffsetController _transformOffsetController;

    private EffectsCompositePlayer _player;

    private void Awake()
    {
        _player = new EffectsCompositePlayer(
            _effectPrefab,
            _audioSource,
            _cameraAccess,
            _transformOffsetController,
            transform.position,
            transform.rotation,
            transform
            );
    }

    public void PlayEffects(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        _player.PlayEffects();
    }
}
