using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public interface IPlayersCollection
{
    int Count { get; }
    ReadOnlyCollection<Vector3> Positions { get; }
    ReadOnlyCollection<Bounds> BoundsList { get; }
    ReadOnlyCollection<Vector2> InputDirections { get; }

    ReadOnlyCollection<bool> AliveFlags { get; }

    void SetMoveController(IMoveController moveController);
}
