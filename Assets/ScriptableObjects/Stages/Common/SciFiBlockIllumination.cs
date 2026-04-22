using UnityEngine;

[CreateAssetMenu(fileName = "NewSciFiBlockIllumination", menuName = "Scriptable Objects/Stages/Common/SciFiBlockIllumination")]
public class SciFiBlockIllumination : ScriptableObject
{
    public Color color1 = Color.white;
    public Color color2 = Color.white;
    public Color color3 = Color.white;

    public Vector3 offsetVector = Vector3.right;
}
