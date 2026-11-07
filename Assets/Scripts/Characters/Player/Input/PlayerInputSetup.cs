using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputSetup : MonoBehaviour
{
    private static readonly string[] KEYBOARD_SCHEMES = { "KeyboardWASD", "KeyboardArrows" };

    public static bool AreInputDevicesAvailable(int numPlayers)
    {
        return Keyboard.current != null && Keyboard.current.added && numPlayers <= KEYBOARD_SCHEMES.Length;
    }

    public void Setup(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= KEYBOARD_SCHEMES.Length)
        {
            throw new System.ArgumentOutOfRangeException(
                nameof(playerIndex),
                $"Player index must be between 0 and {KEYBOARD_SCHEMES.Length - 1}."
            );
        }
        if (Keyboard.current == null || !Keyboard.current.added)
        {
            throw new System.InvalidOperationException("No keyboard is currently connected.");
        }

        // 現在のキーボードを割り当てる
        var playerInput = GetComponent<PlayerInput>();
        playerInput.SwitchCurrentControlScheme(KEYBOARD_SCHEMES[playerIndex], Keyboard.current);
    }
}
