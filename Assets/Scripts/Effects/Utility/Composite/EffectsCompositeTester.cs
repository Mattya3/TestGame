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

    [SerializeField]
    private Renderer _renderer;

    private EffectsCompositePlayer _player;

    private void Start()
    {
        _player = new EffectsCompositePlayer(
            _effectPrefab,
            _audioSource,
            _cameraAccess,
            _transformOffsetController,
            _renderer,
            transform.position,
            transform.rotation,
            transform
            );
    }

    public void PlayEffects(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _player.PlayEffects();
            return;
        }
        if (context.canceled)
        {
            _player.StopEffects();
            return;
        }
    }
}
