using System.Collections.ObjectModel;

public class ReverseMoveController : IGameMoveController
{
    private readonly ReadOnlyCollection<Player> _players;
    private readonly int _requiredCount;

    public ReverseMoveController(ReadOnlyCollection<Player> players)
    {
        _players = players;
        _requiredCount = players.Count;
    }

    public bool ShouldReverseInput()
    {
        int movingCount = 0;
        for (int i = 0; i < _players.Count; i++)
        {
            if (_players[i].InputDirection.x != 0) movingCount++;
        }
        return movingCount >= _requiredCount;
    }
}
