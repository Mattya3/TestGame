using UnityEngine;
using static Constants;

[RequireComponent(typeof(PlayersCollectionMutableAccess))]
[RequireComponent(typeof(PlayersCollectionReadonlyAccess))]
public class MovementRuleManager : MonoBehaviour
{
    [SerializeField]
    private MovementRuleEffect _movementRuleEffect;

    private IMoveController _moveController;

    private PlayersCollectionMutableAccess _mutablePlayersAccess;
    private PlayersCollectionReadonlyAccess _readonlyPlayersAccess;

    private void Awake()
    {
        _mutablePlayersAccess = GetComponent<PlayersCollectionMutableAccess>();
        _readonlyPlayersAccess = GetComponent<PlayersCollectionReadonlyAccess>();
    }

    public void Initialize()
    {
        _moveController = MoveControllerFactory.Create(_movementRuleEffect, _readonlyPlayersAccess);
        _mutablePlayersAccess.SetMoveController(_moveController);
    }
}
