using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    private const int EXPECTED_COLLIDER_COUNT = 2;
    private const float ALIGNMENT_MARGIN = 0.01f; // コライダーを画面端に揃える際のマージン

    private Collider2D[] _colliders; // カメラのコライダーへの参照
    private bool _isValid = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _colliders = GetComponentsInChildren<Collider2D>();
        if (_colliders.Length != EXPECTED_COLLIDER_COUNT)
        {
            Debug.LogError($"Expected {EXPECTED_COLLIDER_COUNT} colliders, but found {_colliders.Length}");
            return;
        }
        if (_colliders[0] == null || _colliders[1] == null)
        {
            Debug.LogError("One or more colliders are missing.");
            return;
        }
        _isValid = true;
    }

    public void AlignCollidersToViewportEdges(Camera camera)
    {
        if (!_isValid)
            return;

        // Assuming the first collider is for the left edge and the second for the right edge
        var leftColliderBounds = _colliders[0].bounds;
        var rightColliderBounds = _colliders[1].bounds;
        float screenHalfHeight = 2f * camera.orthographicSize;
        float screenHalfWidth = screenHalfHeight * camera.aspect;
        float leftColliderX = camera.transform.position.x - screenHalfWidth / 2f - leftColliderBounds.extents.x - ALIGNMENT_MARGIN;
        float rightColliderX = camera.transform.position.x + screenHalfWidth / 2f + rightColliderBounds.extents.x + ALIGNMENT_MARGIN;

        _colliders[0].transform.position = new Vector3(leftColliderX, camera.transform.position.y, _colliders[0].transform.position.z);
        _colliders[1].transform.position = new Vector3(rightColliderX, camera.transform.position.y, _colliders[1].transform.position.z);
    }

    public void EnableColliders(bool enable)
    {
        // 変化しない場合は早期return
        // 先頭要素のenabled状態を確認することで、全体の状態を推測する
        // 一部だけenableが違う状況は想定しない
        if (!_isValid)
            return;
        if (_colliders[0].enabled == enable)
            return;

        for (int i = 0; i < _colliders.Length; i++)
            _colliders[i].enabled = enable;
    }
}
