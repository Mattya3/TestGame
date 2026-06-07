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

    public int Count
    {
        get
        {
            _ValidateReferences();
            return _reference != null ? _reference.Count : 0;
        }
    }

    public List<Vector3> Positions
    { 
        get
        {
            _ValidateReferences();
            return _reference != null ? _reference.Positions : new List<Vector3>();
        }
    }

    public List<Bounds> BoundsList
    {
        get
        {
            _ValidateReferences();
            return _reference != null ? _reference.BoundsList : new List<Bounds>();
        }
    }

    public List<Vector2> InputDirections
    {
        get
        {
            _ValidateReferences();
            return _reference != null ? _reference.InputDirections : new List<Vector2>();
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
