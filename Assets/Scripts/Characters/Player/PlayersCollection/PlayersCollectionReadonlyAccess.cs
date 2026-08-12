using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class PlayersCollectionReadonlyAccess : AccessComponent<IPlayersCollection>
{
    // 参照がない場合に返す空のコレクションをキャッシュしておく
    private ReadOnlyCollection<Vector3> _emptyPositions = new ReadOnlyCollection<Vector3>(
        new List<Vector3>()
    );
    private ReadOnlyCollection<Bounds> _emptyBoundsList = new ReadOnlyCollection<Bounds>(
        new List<Bounds>()
    );
    private ReadOnlyCollection<Vector2> _emptyInputDirections = new ReadOnlyCollection<Vector2>(
        new List<Vector2>()
    );

    public int Count => Reference != null ? Reference.Count : 0;

    public ReadOnlyCollection<Vector3> Positions =>
        Reference != null ? Reference.Positions : _emptyPositions;

    public ReadOnlyCollection<Bounds> BoundsList =>
        Reference != null ? Reference.BoundsList : _emptyBoundsList;

    public ReadOnlyCollection<Vector2> InputDirections =>
        Reference != null ? Reference.InputDirections : _emptyInputDirections;
}
