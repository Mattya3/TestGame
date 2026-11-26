using UnityEngine;

[CreateAssetMenu(fileName = "ScreenResolution", menuName = "Scriptable Objects/UI/ScreenResolution")]
public class ScreenResolution : ScriptableObject
{
    [SerializeField]
    private Vector2Int _resolution = new Vector2Int(1920, 1080);

    public Vector2Int Resolution => _resolution;

    public float AspectRatio => (float)_resolution.x / _resolution.y;
}
