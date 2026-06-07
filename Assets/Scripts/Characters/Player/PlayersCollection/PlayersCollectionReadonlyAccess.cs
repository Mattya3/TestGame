using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public int Count
    {
        get
        {
            _ValidateReferences();
            return _reference != null ? _reference.Count : 0;
        }
    }

    public ReadOnlyCollection<Vector3> Positions
    {
        get
        {
            _ValidateReferences();
            return _reference != null
                ? _reference.Positions
                : new ReadOnlyCollection<Vector3>(new List<Vector3>());
        }
    }

    public ReadOnlyCollection<Bounds> BoundsList
    {
        get
        {
            _ValidateReferences();
            return _reference != null
                ? _reference.BoundsList
                : new ReadOnlyCollection<Bounds>(new List<Bounds>());
        }
    }

    public ReadOnlyCollection<Vector2> InputDirections
    {
        get
        {
            _ValidateReferences();
            return _reference != null
                ? _reference.InputDirections
                : new ReadOnlyCollection<Vector2>(new List<Vector2>());
        }
    }

    private void _ValidateReferences()
    {
        if (_reference == null)
        {
            Debug.LogError(
                "No IPlayersCollection reference registered. Please ensure that an IPlayersCollection implementation is registered before using PlayersCollectionReadonlyAccess."
            );
        }
    }
}
