using UnityEngine;

public class CameraBoundsBase : MonoBehaviour, ICameraBounds
{
    private const float LINE_LENGTH = 1e2f;

    [SerializeField]
    private bool _freezeX = false; // X座標を初期位置で固定するフラグ

    [SerializeField]
    private bool _freezeY = false; // Y座標を初期位置で固定するフラグ

    public virtual Vector3 Bound(Vector3 pos, Vector3 originalPos, Camera camera)
    {
        return new Vector3(
            _freezeX ? originalPos.x : pos.x,
            _freezeY ? originalPos.y : pos.y,
            pos.z
        );
    }

    public virtual void DrawGizmos(Camera camera, Vector3 originalPos)
    {
        Gizmos.color = Color.green;
        if (_freezeX)
            Gizmos.DrawLine(originalPos + Vector3.up * LINE_LENGTH, originalPos + Vector3.down * LINE_LENGTH);
        if (_freezeY)
            Gizmos.DrawLine(originalPos + Vector3.left * LINE_LENGTH, originalPos + Vector3.right * LINE_LENGTH);
    }

    protected bool FreezeX => _freezeX;
    protected bool FreezeY => _freezeY;
}
