using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[RequireComponent(typeof(GameEventTriggerAccess))]
public class GameManager : MonoBehaviour, IGameManager
{
    [SerializeField]
    private ExternalEffectManager _externalEffectManager;

    [SerializeField]
    private PlayersManager _playersManager;

    private GameEventTriggerAccess _gameEventTriggerAccess;

    private void Awake()
    {
        _gameEventTriggerAccess = GetComponent<GameEventTriggerAccess>();
        AccessComponent<IGameManager>.RegisterReference(this);
    }

    private void OnDestroy()
    {
        AccessComponent<IGameManager>.UnregisterReference(this);
    }

    private IEnumerator Start()
    {
        if (_externalEffectManager == null)
        {
            Debug.LogError("GameManager dependencies are not properly set up.", this);
            yield break;
        }

        yield return new WaitUntil(() => _playersManager.Players.Count == Constants.PLAYER_COUNT);
        _externalEffectManager.Initialize(_playersManager.Players);
    }

    public void HandlePlayStart()
    {
        _gameEventTriggerAccess.TriggerEventActions(GameEvent.GamePlayStart);
    }

    public void HandleFailure()
    {
        _gameEventTriggerAccess.TriggerEventActions(GameEvent.Failure);
    }

    public void HandleSuccess()
    {
        _gameEventTriggerAccess.TriggerEventActions(GameEvent.Success);
    }

    public void HandleSceneEnd()
    {
        _gameEventTriggerAccess.TriggerEventActions(GameEvent.SceneEnd);
    }
}
