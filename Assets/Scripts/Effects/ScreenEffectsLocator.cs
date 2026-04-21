using UnityEngine;

public static class ScreenEffectsLocator
{
    private static IScreenEffects _screenEffects;

    public static void Provide(IScreenEffects screenEffects)
    {
        _screenEffects = screenEffects;
    }

    public static IScreenEffects Get()
    {
        return _screenEffects;
    }

    public static void Clear()
    {
        _screenEffects = null;
    }
}