using System.Collections.Generic;
using UnityEngine;

public class PlayersCollectionReadonlyAccess : MonoBehaviour
{
    private static IPlayersCollection _reference;

    public static void Register(IPlayersCollection reference)
    {
        _reference = reference;
    }

    public static void Unregister(IPlayersCollection reference)
    {
        if (_reference != reference)
            return;

        _reference = null;
    }

    public int Count => _reference?.Count ?? 0;

    public List<Vector3> Positions => _reference?.Positions ?? new List<Vector3>();

    public List<Bounds> BoundsList => _reference?.BoundsList ?? new List<Bounds>();

    public List<Vector2> InputDirections => _reference?.InputDirections ?? new List<Vector2>();
}
