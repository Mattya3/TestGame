using UnityEngine;
using System;
using System.Collections.Generic;

public class CharacterEffectsPlayer : MonoBehaviour
{
    [Serializable]
    private struct KeyedEffectPrefab
    {
        [SerializeField]
        private string _key;

        [SerializeField]
        private GameObject _effectPrefab;

        public EffectsCompositePlayer CreatePlayer(
            AudioSource audioSource,
            CameraMutableAccess cameraAccess,
            TransformOffsetController transformOffsetController,
            Renderer renderer,
            Vector3 position,
            Quaternion rotation,
            Transform parent
            )
        {
            return new EffectsCompositePlayer(
                _effectPrefab,
                audioSource,
                cameraAccess,
                transformOffsetController,
                renderer,
                position,
                rotation,
                parent
            );
        }

        public string Key => _key;
    }

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private CameraMutableAccess _cameraAccess;

    [SerializeField]
    private TransformOffsetController _transformOffsetController;

    [SerializeField]
    private Renderer _renderer;

    [SerializeField]
    private List<KeyedEffectPrefab> _effectPrefabs = new List<KeyedEffectPrefab>();

    private Dictionary<string, EffectsCompositePlayer> _effectPlayers = new Dictionary<string, EffectsCompositePlayer>();

    private void Awake()
    {
        foreach (var keyedPrefab in _effectPrefabs)
        {
            if (_effectPlayers.ContainsKey(keyedPrefab.Key))
            {
                Debug.LogError($"Duplicate effect key found: {keyedPrefab.Key}. Each effect must have a unique key.");
                continue;
            }

            var player = keyedPrefab.CreatePlayer(
                _audioSource,
                _cameraAccess,
                _transformOffsetController,
                _renderer,
                transform.position,
                transform.rotation,
                transform
            );
            _effectPlayers[keyedPrefab.Key] = player;
        }
    }

    private void OnDestroy()
    {
        foreach (var player in _effectPlayers.Values)
            player.Cleanup();
    }

    public void Play(string key)
    {
        if (_effectPlayers.TryGetValue(key, out var player))
            player.PlayEffects();
        else
            Debug.LogError($"Effect with key '{key}' not found.");
    }
}
