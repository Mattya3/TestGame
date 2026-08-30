using UnityEngine;
using UnityEngine.InputSystem;

public class EffectsCompositeTester : MonoBehaviour
{
    [SerializeField]
    private GameObject _effectPrefab;

    private EffectsCompositePlayer _player;

    private void Awake()
    {
        _player = new EffectsCompositePlayer(
            _effectPrefab,
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
