using UnityEngine;

[CreateAssetMenu(fileName = "UIColor", menuName = "Scriptable Objects/UI/UIColor")]
public class UIColor : ScriptableObject
{
    [SerializeField]
    private Color _color;

    public Color Color => _color;
}
