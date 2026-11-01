using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CreatePlayerActionConfiguration")]
public class PlayerActionConfiguration : ScriptableObject
{
    [SerializeField, Range(0f, 20f)]
    public float _moveSpeed;

    [SerializeField, Range(0f, 40f)]
    public float _jumpInitialVelocity;
}
