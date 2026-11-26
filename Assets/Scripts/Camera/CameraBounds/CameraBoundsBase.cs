using UnityEngine;

public class CameraBoundsBase : MonoBehaviour, ICameraBounds
{
    private const float LINE_LENGTH = 1e2f;

    [SerializeField]
    private bool _freezeX = false; // X座標を初期位置で固定するフラグ

    [SerializeField]
    private bool _freezeY = false; // Y座標を初期位置で固定するフラグ

    protected virtual void Awake()
    {
        AccessComponent<ICameraBounds>.RegisterReference(this);
    }

    protected virtual void OnDestroy()
    {
        AccessComponent<ICameraBounds>.UnregisterReference(this);
    }

    private void OnDrawGizmosSelected()
    {
        DrawGizmos();
    }

    public virtual void EnsureInitialized() {}

    public virtual Vector3 Bound(Vector3 pos, Vector3 originalPos, Camera camera)
    {
        return new Vector3(
            _freezeX ? originalPos.x : pos.x,
            _freezeY ? originalPos.y : pos.y,
            pos.z
        );
    }

    public virtual void DrawGizmos() {}

    protected bool FreezeX => _freezeX;
    protected bool FreezeY => _freezeY;
}
