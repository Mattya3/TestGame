using UnityEngine;
using UnityEngine.InputSystem;

public class MouseTracker : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;

    private bool _isDragging = false;
    private Vector2 _mousePosition;

    public void OnMouseClicked(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isDragging = true;
            _TrackMousePosition();
        }
        else if (context.canceled)
        {
            _isDragging = false;
        }
    }

    public void OnMouseMoved(InputAction.CallbackContext context)
    {
        _mousePosition = context.ReadValue<Vector2>();

        if (_isDragging)
            _TrackMousePosition();
    }

    private void _TrackMousePosition()
    {
        var worldPosition = _camera.ScreenToWorldPoint(new Vector3(_mousePosition.x, _mousePosition.y, _camera.nearClipPlane));
        transform.position = new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
    }
}
