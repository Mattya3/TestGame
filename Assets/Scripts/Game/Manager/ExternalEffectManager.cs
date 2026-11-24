using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayersCollectionReadonlyAccess))]
[RequireComponent(typeof(PlayersCollectionMutableAccess))]
public class ExternalEffectManager : MonoBehaviour
{
    [SerializeField]
    private Constants.ExternalEffectType _player1ExternalEffectType = Constants
        .ExternalEffectType
        .None;

    [SerializeField]
    private Constants.ExternalEffectType _player2ExternalEffectType = Constants
        .ExternalEffectType
        .None;

    private PlayersCollectionReadonlyAccess _playersReadonlyAccess;
    private PlayersCollectionMutableAccess _playersMutableAccess;

    private void Awake()
    {
        _playersReadonlyAccess = GetComponent<PlayersCollectionReadonlyAccess>();
        _playersMutableAccess = GetComponent<PlayersCollectionMutableAccess>();
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < _playersMutableAccess.Count; i++)
        {
            Constants.ExternalEffectType effectType = _GetExternalEffectType(i);
            IExternalEffectTarget target = _playersMutableAccess.CreatePlayerExternalEffectTarget(i);
            if (target == null)
            {
                Debug.LogError($"Failed to create external effect target for player at index {i}.", this);
                continue;
            }
            IExternalEffect externalEffect = ExternalEffectFactory.Create(effectType, _playersReadonlyAccess, target);
            _playersMutableAccess.SetExternalEffect(i, externalEffect);
        }
    }

    private Constants.ExternalEffectType _GetExternalEffectType(int playerIndex)
    {
        if (playerIndex == 0)
            return _player1ExternalEffectType;
        if (playerIndex == 1)
            return _player2ExternalEffectType;

        return Constants.ExternalEffectType.None;
    }
}
