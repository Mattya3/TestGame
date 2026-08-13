using System.Collections.ObjectModel;
using UnityEngine;

public interface IHoleMaskTarget
{
    public ReadOnlyCollection<bool> AreEnabled { get; }

    public ReadOnlyCollection<Vector3> ScreenPositions { get; }
}
