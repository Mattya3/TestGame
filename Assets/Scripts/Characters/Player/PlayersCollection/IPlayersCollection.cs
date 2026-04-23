
using System.Collections.Generic;
using UnityEngine;

public interface IPlayersCollection
{
    int Count { get; }
    List<Vector3> Positions { get; }
    List<Bounds> BoundsList { get; }
    List<Vector2> InputDirections { get; }

    void SetMoveController(IMoveController moveController);
}
