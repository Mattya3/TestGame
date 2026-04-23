using System.Collections.Generic;
using UnityEngine;
using static Constants;

[RequireComponent(typeof(PlayersCollectionAccess))]
[RequireComponent(typeof(PlayersCollectionReadonlyAccess))]
public class MovementRuleManager : MonoBehaviour
{
    [SerializeField]
    private MovementRuleEffect _movementRuleEffect;

    private IMoveController _moveController;

    private PlayersCollectionAccess _players;
    private PlayersCollectionReadonlyAccess _readonlyPlayers;

    private void Awake()
    {
        _players = GetComponent<PlayersCollectionAccess>(); 
        _readonlyPlayers = GetComponent<PlayersCollectionReadonlyAccess>();
    }

    public void Initialize()
    {
        _moveController = MoveControllerFactory.Create(_movementRuleEffect, _readonlyPlayers);
        _players.SetMoveController(_moveController);
    }
}
