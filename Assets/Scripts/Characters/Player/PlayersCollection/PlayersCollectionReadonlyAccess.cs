using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class PlayersCollectionReadonlyAccess : AccessComponent<IPlayersCollection>
{
    private ReadOnlyCollection<bool> _emptyAliveFlags = new ReadOnlyCollection<bool>(new List<bool>());

    public int Count => Reference != null ? Reference.Count : 0;

    public ReadOnlyCollection<Vector3> Positions =>
        Reference != null
            ? Reference.Positions
            : new ReadOnlyCollection<Vector3>(new List<Vector3>());

    public ReadOnlyCollection<Bounds> BoundsList =>
        Reference != null
            ? Reference.BoundsList
            : new ReadOnlyCollection<Bounds>(new List<Bounds>());

    public ReadOnlyCollection<Vector2> InputDirections =>
        Reference != null
            ? Reference.InputDirections
            : new ReadOnlyCollection<Vector2>(new List<Vector2>());

    public ReadOnlyCollection<bool> AliveFlags =>
        Reference != null
            ? Reference.AliveFlags
            : _emptyAliveFlags;
}
