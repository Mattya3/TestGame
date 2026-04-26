using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundScroller : MonoBehaviour
{
    [SerializeField]
    private Transform _cameraTransform;

    [SerializeField]
    private Vector3 _followFactor = new Vector3(0.5f, 0.5f, 0);

    private Vector2 _spriteSize;
    private Vector3 _previousPosition;

    private void Awake()
    {
        if (_cameraTransform == null)
        {
            Debug.LogError("Camera Transform is not assigned.", this);
            enabled = false;
            return;
        }

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

    void Start()
    {
        _previousPosition = _cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 delta = _cameraTransform.position - _previousPosition;
        transform.position += Vector3.Scale(delta, _followFactor);
        _previousPosition = _cameraTransform.position;

        if (Mathf.Abs(transform.position.x - _cameraTransform.position.x) >= _spriteSize.x)
        {
            float offsetX = (transform.position.x - _cameraTransform.position.x) % _spriteSize.x;
            transform.position = new Vector3(
                _cameraTransform.position.x + offsetX,
                transform.position.y,
                transform.position.z
            );
        }
        if (Mathf.Abs(transform.position.y - _cameraTransform.position.y) >= _spriteSize.y)
        {
            float offsetY = (transform.position.y - _cameraTransform.position.y) % _spriteSize.y;
            transform.position = new Vector3(
                transform.position.x,
                _cameraTransform.position.y + offsetY,
                transform.position.z
            );
        }
    }
}
