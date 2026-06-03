using UnityEngine;
using static Constants;

[RequireComponent(typeof(PlayersCollectionMutableAccess))]
[RequireComponent(typeof(PlayersCollectionReadonlyAccess))]
public class MovementRuleManager : MonoBehaviour
{
    [SerializeField]
    private MovementRuleEffect _movementRuleEffect;

    private IMoveController _moveController;

    private PlayersCollectionMutableAccess _mutablePlayers;
    private PlayersCollectionReadonlyAccess _readonlyPlayers;

    private void Awake()
    {
        _mutablePlayers = GetComponent<PlayersCollectionMutableAccess>(); 
        _readonlyPlayers = GetComponent<PlayersCollectionReadonlyAccess>();
    }

    public void Initialize()
    {
        _moveController = MoveControllerFactory.Create(_movementRuleEffect, _readonlyPlayers);
        _mutablePlayers.SetMoveController(_moveController);
    }
}
