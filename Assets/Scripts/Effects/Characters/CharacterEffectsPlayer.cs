using UnityEngine;
using System;
using System.Collections.Generic;

public class CharacterEffectsPlayer : MonoBehaviour
{
    private static readonly int FACING_DIRECTION_PROPERTY_ID = Shader.PropertyToID("_FacingDirection");

    private static Vector2 _FacingDirectionToVector(Character.FacingDirectionType facingDirection)
    {
        return facingDirection == Character.FacingDirectionType.Right ? Vector2.right : Vector2.left;
    }

    [Serializable]
    private struct KeyedEffectPrefab
    {
        [SerializeField]
        private string _key;

        [SerializeField]
        private GameObject _effectPrefab;

        public EffectsCompositePlayer CreatePlayer(
            SoundEffectOutput soundEffectOutput,
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
                soundEffectOutput,
                cameraAccess,
                transformOffsetController,
                renderer,
                position,
                rotation,
                parent
            );
        }

        public string Key => _key;

        public bool HasValidPrefab => _effectPrefab != null;
    }

    [SerializeField]
    private SoundEffectOutput _soundEffectOutput;

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
            if (keyedPrefab.Key == null || keyedPrefab.Key.Trim() == "")
            {
                Debug.LogError("Effect key cannot be null or empty.");
                continue;
            }
            if (_effectPlayers.ContainsKey(keyedPrefab.Key))
            {
                Debug.LogError($"Duplicate effect key found: {keyedPrefab.Key}. Each effect must have a unique key.");
                continue;
            }
            if (!keyedPrefab.HasValidPrefab)
            {
                Debug.LogError($"Effect prefab for key '{keyedPrefab.Key}' is not set.");
                continue;
            }

            var player = keyedPrefab.CreatePlayer(
                _soundEffectOutput,
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

    public void Play(string key, Character.FacingDirectionType facingDirection)
    {
        if (_effectPlayers.TryGetValue(key, out var player))
        {
            player.SetVFXVector2Property(FACING_DIRECTION_PROPERTY_ID, _FacingDirectionToVector(facingDirection));
            player.PlayEffects();
        }
        else
        {
            Debug.LogError($"Effect with key '{key}' not found.");
        }
    }
}
