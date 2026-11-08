using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CameraReadonlyAccess))]
public class ParallaxScroller : MonoBehaviour
{
    [SerializeField]
    private Vector3 _followFactor = new Vector3(0.5f, 0.5f, 0);

    [SerializeField]
    private bool _loopX = true;

    [SerializeField]
    private bool _loopY = true;

    private CameraReadonlyAccess _cameraAccess;
    private Vector2 _spriteSize;
    private Vector3 _previousPosition;

    private void Awake()
    {
        _cameraAccess = GetComponent<CameraReadonlyAccess>();

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Sprite sprite = sr.sprite;
        if (sprite == null)
        {
            Debug.LogError("SpriteRenderer does not have a sprite assigned.", this);
            enabled = false;
            return;
        }

        _spriteSize = new Vector2(
            sprite.rect.width / sprite.pixelsPerUnit * transform.lossyScale.x,
            sprite.rect.height / sprite.pixelsPerUnit * transform.lossyScale.y
        );
    }

    private void Start()
    {
        var currentCameraPosition = _cameraAccess.UnoffsetPosition;
        _previousPosition = currentCameraPosition;

        // シーンのエディタ上でカメラが原点に配置されることを想定
        // そこからの相対位置を基準として動かす
        transform.position += Vector3.Scale(currentCameraPosition, _followFactor);
    }

    void LateUpdate()
    {
        var currentCameraPosition = _cameraAccess.UnoffsetPosition;
        Vector3 delta = currentCameraPosition - _previousPosition;
        transform.position += Vector3.Scale(delta, _followFactor);
        _previousPosition = currentCameraPosition;

        if (
            _loopX
            && Mathf.Abs(transform.position.x - _cameraAccess.UnoffsetPosition.x) >= _spriteSize.x
        )
        {
            float offsetX = (transform.position.x - _cameraAccess.UnoffsetPosition.x) % _spriteSize.x;
            transform.position = new Vector3(
                _cameraAccess.UnoffsetPosition.x + offsetX,
                transform.position.y,
                transform.position.z
            );
        }
        if (
            _loopY
            && Mathf.Abs(transform.position.y - _cameraAccess.UnoffsetPosition.y) >= _spriteSize.y
        )
        {
            float offsetY = (transform.position.y - _cameraAccess.UnoffsetPosition.y) % _spriteSize.y;
            transform.position = new Vector3(
                transform.position.x,
                _cameraAccess.UnoffsetPosition.y + offsetY,
                transform.position.z
            );
        }
    }
}
