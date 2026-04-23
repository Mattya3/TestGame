using UnityEngine;

public class ReverseMoveController : IMoveController
{
    private PlayersCollectionReadonlyAccess _players;
    private readonly int _requiredCount;

    public ReverseMoveController(PlayersCollectionReadonlyAccess players)
    {
        _players = players;
        _requiredCount = players.Count;
    }

    public Vector2 ConvertInputDirection(Vector2 rawInput)
    {
        return _ShouldReverseInput() ? new Vector2(-rawInput.x, rawInput.y) : rawInput;
    }

    private bool _ShouldReverseInput()
    {
        var inputDirections = _players.InputDirections;

        int movingCount = 0;
        for (int i = 0; i < inputDirections.Count; i++)
        {
            if (inputDirections[i].x != 0)
                movingCount++;
        }
        return movingCount >= _requiredCount;
    }
}
