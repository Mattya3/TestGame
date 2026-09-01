using UnityEngine;

public class EffectsCompositePlayer
{
    private GameObject _instance;
    private IEffectsCompositor[] _compositors;

    public EffectsCompositePlayer(
        GameObject effectPrefab,
        AudioSource audioSource,
        CameraMutableAccess cameraAccess,
        TransformOffsetController transformOffsetController,
        Renderer renderer,
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
        _compositors = _instance.GetComponentsInChildren<IEffectsCompositor>();
        if (_compositors == null || _compositors.Length == 0)
        {
            Debug.LogWarning("EffectsCompositor components are not found on the effect prefab.");
            return;
        }

        foreach (var compositor in _compositors)
        {
            compositor.Initialize(audioSource, cameraAccess, transformOffsetController, renderer);
        }
    }

    public void PlayEffects()
    {
        foreach (var compositor in _compositors)
            compositor.PlayEffects();
    }

    public void StopEffects()
    {
        foreach(var compositor in _compositors)
            compositor.StopEffects();
    }

    public void Cleanup()
    {
        if (_instance != null)
        {
            GameObject.Destroy(_instance);
            _instance = null;
        }
    }
}
