using UnityEngine;
using static Constants;

public class GroundDetector : MonoBehaviour
{
    private const float GROUND_CHECK_THICKNESS = 0.005f;
    private const float GROUND_MOVE_MARGIN = 0.001f;

    private Collider2D _collider;
    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _collider = GetComponentInParent<Collider2D>();
        _rigidBody = GetComponentInParent<Rigidbody2D>();
    }

    public bool IsGrounded()
    {
        return _GetValidGroundHit(Layers.SOLID, Layers.PLAYER).collider != null;
    }

    public Vector2 GetGroundVelocity()
    {
        RaycastHit2D hit = _GetValidGroundHit(Layers.PLAYER);
        if (hit.collider == null || hit.collider.attachedRigidbody == null)
            return Vector2.zero;

        Rigidbody2D groundRigidbody = hit.collider.attachedRigidbody;
        return groundRigidbody.GetPointVelocity(_collider.bounds.center);
    }

    private RaycastHit2D _GetValidGroundHit(params string[] layerNames)
    {
        RaycastHit2D hit = _GroundCheck(layerNames);
        if (hit.collider == null)
            return hit;

        Rigidbody2D groundRigidbody = hit.collider.attachedRigidbody;
        if (groundRigidbody == null)
            return hit;

        float groundVelocityY = groundRigidbody.GetPointVelocity(_collider.bounds.center).y;
        float relativeVy = _rigidBody.linearVelocity.y - groundVelocityY;

        // 相対上向き速度が閾値（GROUND_MOVE_MARGIN）を上回る場合は接地していないものとみなす
        if (relativeVy > GROUND_MOVE_MARGIN)
            return default;
        return hit;
    }

    private RaycastHit2D _GroundCheck(params string[] layerNames)
    {
        Bounds bounds = _collider.bounds;
        Vector2 origin = new Vector2(bounds.center.x, bounds.min.y - GROUND_CHECK_THICKNESS * 2);
        Vector2 boxSize = new Vector2(bounds.size.x, GROUND_CHECK_THICKNESS);
        int mask = LayerMask.GetMask(layerNames);
        return Physics2D.BoxCast(origin, boxSize, 0f, Vector2.down, GROUND_CHECK_THICKNESS, mask);
    }
}
