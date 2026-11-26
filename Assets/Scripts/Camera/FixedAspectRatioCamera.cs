using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FixedAspectRatioCamera : MonoBehaviour
{
    [SerializeField]
    private ScreenResolution _referenceResolution;

    private Camera _camera;
    private int _previousWidth;
    private int _previousHeight;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        UpdateViewport();
    }

    private void Update()
    {
        if (Screen.width != _previousWidth || Screen.height != _previousHeight)
            UpdateViewport();
    }

    private void UpdateViewport()
    {
        _previousWidth = Screen.width;
        _previousHeight = Screen.height;

        float targetAspect = _referenceResolution.AspectRatio;
        float screenAspect = (float)Screen.width / Screen.height;
        Rect rect = new Rect(0f, 0f, 1f, 1f);

        if (screenAspect > targetAspect)
        {
            // 左右に黒帯
            rect.width = targetAspect / screenAspect;
            rect.x = (1f - rect.width) * 0.5f;
        }
        else
        {
            // 上下に黒帯
            rect.height = screenAspect / targetAspect;
            rect.y = (1f - rect.height) * 0.5f;
        }

        _camera.rect = rect;
        _camera.aspect = targetAspect;
    }
}