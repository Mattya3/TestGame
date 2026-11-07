using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputSetup : MonoBehaviour
{
    private static readonly string[] KEYBOARD_SCHEMES =
    {
        "KeyboardWASD",
        "KeyboardArrows",
    };

    public void Setup(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= KEYBOARD_SCHEMES.Length)
        {
            Debug.LogError($"Invalid player index: {playerIndex}. It should be between 0 and {KEYBOARD_SCHEMES.Length - 1}.");
            return;
        }

        // 現在のキーボードを割り当てる
        var playerInput = GetComponent<PlayerInput>();
        playerInput.SwitchCurrentControlScheme(KEYBOARD_SCHEMES[playerIndex], Keyboard.current);
    }
}
