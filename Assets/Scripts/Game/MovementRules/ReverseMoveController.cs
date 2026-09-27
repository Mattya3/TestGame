using UnityEngine;

public class ReverseMoveController : IMoveController
{
    private PlayersCollectionReadonlyAccess _playersAccess;
    private readonly int _requiredCount;

    public ReverseMoveController(PlayersCollectionReadonlyAccess playersAccess)
    {
        _playersAccess = playersAccess;
        _requiredCount = playersAccess.Count;
    }

    public Vector2 ConvertInputDirection(Vector2 rawInput)
    {
        return _ShouldReverseInput() ? new Vector2(-rawInput.x, rawInput.y) : rawInput;
    }

    private bool _ShouldReverseInput()
    {
        var inputDirections = _playersAccess.InputDirections;

        int movingCount = 0;
        for (int i = 0; i < inputDirections.Count; i++)
        {
            if (inputDirections[i].x != 0)
                movingCount++;
        }
        return movingCount >= _requiredCount;
    }
}
