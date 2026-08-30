using UnityEngine;

public class EffectsCompositPlayer
{
    private GameObject _instance;
    private EffectsCompositor _compositor;

    public EffectsCompositPlayer(
        GameObject effectPrefab,
        Vector3 position = default,
        Quaternion rotation = default,
        Transform parent = null
        )
    {
        if (effectPrefab == null)
        {
            Debug.LogWarning("Effect prefab is null");
            return;
        }
        // プレハブであるかを確認
        if (effectPrefab.scene.IsValid())
        {
            Debug.LogWarning("Effect prefab is not a prefab");
            return;
        }

        _instance = GameObject.Instantiate(effectPrefab, position, rotation, parent);
        _compositor = _instance.GetComponent<EffectsCompositor>();
        if (_compositor == null)
        {
            Debug.LogWarning("EffectsCompositor component is not found on the effect prefab.");
            return;
        }

        _compositor.Initialize();
    }

    public void PlayEffects()
    {
        _compositor?.PlayEffects();
    }
}
