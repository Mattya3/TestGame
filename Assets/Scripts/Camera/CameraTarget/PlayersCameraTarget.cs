using UnityEngine;

[RequireComponent(typeof(PlayersCollectionReadonlyAccess))]
public class PlayersCameraTarget : MonoBehaviour, ICameraTarget
{
    [SerializeField]
    private Vector3 _offset;

    [SerializeField]
    private CameraTargetShift _shift = new CameraTargetShift();

    [SerializeField]
    private CameraTargetShiftDamp _shiftDamp = new CameraTargetShiftDamp();

    private PlayersCollectionReadonlyAccess _players;
    private Vector3 _position = Vector3.zero;

    void Awake()
    {
        if (_offset.z >= 0)
        {
            Debug.LogError("カメラのoffset.zは負の値でなければなりません", this);
            enabled = false;
            return;
        }
        _shift.Awake();

        _players = GetComponent<PlayersCollectionReadonlyAccess>();
    }

    public void OnStart()
    {
        enabled = true;

        var center = _CalculateCenter();
        _shift.Start(center);
        _position = center + _offset + _shift.Get();
    }

    void FixedUpdate()
    {
        var center = _CalculateCenter();
        var damp = _shiftDamp.CalculateDamp(_players);

        _shift.FixedUpdate(center, damp);
        _position = center + _offset + _shift.Get();
    }

    private Vector3 _CalculateCenter()
    {
        var playerPositions = _players.Positions;

        if (playerPositions == null || playerPositions.Count == 0)
            return Vector3.zero;

        var sum = Vector3.zero;
        for (int i = 0; i < playerPositions.Count; i++)
        {
            sum += playerPositions[i];
        }
        return sum / playerPositions.Count;
    }

    public Vector3 Position => _position;

    public bool EnableCollider => true;
}
