using UnityEngine;
using UnityEngine.InputSystem;

public class EffectsCompositTester : MonoBehaviour
{
    [SerializeField]
    private GameObject _effectPrefab;

    private EffectsCompositPlayer _player;

    private void Awake()
    {
        _player = new EffectsCompositPlayer(
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
