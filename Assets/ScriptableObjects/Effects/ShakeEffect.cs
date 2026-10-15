using UnityEngine;

[CreateAssetMenu(fileName = "ShakeEffect", menuName = "Scriptable Objects/Effects/ShakeEffect")]
public class ShakeEffect : ScriptableObject
{
    public enum ShakeType
    {
        OneShot,
        Continuous,
    }

    public enum ShakeUpdateMode
    {
        Normal,
        UnscaledTime,
    }

    [SerializeField]
    private ShakeType _shakeType = ShakeType.OneShot;

    [SerializeField]
    private ShakeUpdateMode _updateMode = ShakeUpdateMode.Normal;

    [SerializeField, Min(1e-6f)]
    private float _duration = 1.0f;

    [SerializeField]
    private Vector2 _magnitude = new Vector2(0.1f, 0.1f);

    [SerializeField]
    private Vector2 _frequency = new Vector2(1.0f, 1.0f);

    [SerializeField]
    private Vector2 _phaseOffsets = new Vector2(0.0f, 0.0f);

    public ShakeType Type => _shakeType;
    public ShakeUpdateMode UpdateMode => _updateMode;
    public float Duration => _duration;
    public Vector2 Magnitude => _magnitude;
    public Vector2 Frequency => _frequency;
    public Vector2 PhaseOffsets => _phaseOffsets;
}
