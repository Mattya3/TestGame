using UnityEngine;

[RequireComponent(typeof(CameraReadonlyAccess))]
public class HoleMaskController : MonoUIImageMaterialAccessBehaviour
{
    private const int MAX_NUM_TARGETS = 2;
    private static readonly Vector2 CENTER_BOUNDS_MIN = new(-0.045f, -0.08f);
    private static readonly Vector2 CENTER_BOUNDS_MAX = new(1.045f, 1.08f);

    [SerializeField, HideInInspector]
    private float _maskThreshold = 0.0f;

    [SerializeField]
    private UIColor _color;

    private float _lastMaskThreshold = float.MaxValue;
    private CameraReadonlyAccess _cameraAccess;
    private IHoleMaskTarget _target;

    private static readonly int ColorID = Shader.PropertyToID("_BaseColor");
    private static readonly int MaskThresholdID = Shader.PropertyToID("_MaskThreshold");
    private static readonly int CameraAspectID = Shader.PropertyToID("_CameraAspect");
    private static readonly int ViewportRectPositionID = Shader.PropertyToID("_ViewportRectPosition");
    private static readonly int ViewportRectSizeID = Shader.PropertyToID("_ViewportRectSize");
    private static readonly int[] TargetViewportPositionIDs = new int[MAX_NUM_TARGETS]
    {
        Shader.PropertyToID("_TargetViewportPosition1"),
        Shader.PropertyToID("_TargetViewportPosition2"),
    };
    private static readonly int[] TargetEnabledIDs = new int[MAX_NUM_TARGETS]
    {
        Shader.PropertyToID("_TargetEnabled1"),
        Shader.PropertyToID("_TargetEnabled2"),
    };

    protected override void Awake()
    {
        base.Awake();

        _cameraAccess = GetComponent<CameraReadonlyAccess>();
        _target = GetComponent<IHoleMaskTarget>();

        if (_target == null)
        {
            Debug.LogError(
                $"HoleMaskController requires a component that implements IHoleMaskTarget on the same GameObject."
            );
            enabled = false;
        }
    }

    protected override bool IsMaterialValid(Material material)
    {
        if (!material.HasProperty(ColorID))
        {
            Debug.LogError(
                $"Material {material.name} does not have a property named '_Color'."
            );
            return false;
        }
        if (!material.HasProperty(MaskThresholdID))
        {
            Debug.LogError(
                $"Material {material.name} does not have a property named '_MaskThreshold'."
            );
            return false;
        }
        if (!material.HasProperty(CameraAspectID))
        {
            Debug.LogError(
                $"Material {material.name} does not have a property named '_CameraAspect'."
            );
            return false;
        }
        if (!material.HasProperty(ViewportRectPositionID))
        {
            Debug.LogError(
                $"Material {material.name} does not have a property named '_ViewportRectPosition'."
            );
            return false;
        }
        if (!material.HasProperty(ViewportRectSizeID))
        {
            Debug.LogError(
                $"Material {material.name} does not have a property named '_ViewportRectSize'."
            );
            return false;
        }
        for (int i = 0; i < MAX_NUM_TARGETS; i++)
        {
            if (!material.HasProperty(TargetViewportPositionIDs[i]))
            {
                Debug.LogError(
                    $"Material {material.name} does not have a property named 'TargetViewportPosition{i + 1}'."
                );
                return false;
            }
            if (!material.HasProperty(TargetEnabledIDs[i]))
            {
                Debug.LogError(
                    $"Material {material.name} does not have a property named 'TargetEnabled{i + 1}'."
                );
                return false;
            }
        }
        return true;
    }

    protected override void SetMaterialProperties(Material material)
    {
        material.SetColor(ColorID, _color?.Color ?? Color.black);
        material.SetFloat(MaskThresholdID, _maskThreshold);
        _lastMaskThreshold = _maskThreshold;

        // エディタ上ではオブジェクトを参照せずデフォルトの値を設定
        if (!Application.isPlaying)
            return;

        material.SetFloat(CameraAspectID, _cameraAccess.AspectRatio);
        material.SetVector(ViewportRectPositionID, new Vector2(_cameraAccess.ViewportRect.x, _cameraAccess.ViewportRect.y));
        material.SetVector(ViewportRectSizeID, new Vector2(_cameraAccess.ViewportRect.width, _cameraAccess.ViewportRect.height));

        var enabledList = _target.AreEnabled;
        var screenPositionsList = _target.ViewportPositions;
        var numTargets = Mathf.Min(enabledList.Count, screenPositionsList.Count, MAX_NUM_TARGETS);
        for (int i = 0; i < MAX_NUM_TARGETS; i++)
        {
            var position = i < numTargets ? screenPositionsList[i] : Vector3.zero;
            var clampedPosition = new Vector3(
                Mathf.Clamp(position.x, CENTER_BOUNDS_MIN.x, CENTER_BOUNDS_MAX.x),
                Mathf.Clamp(position.y, CENTER_BOUNDS_MIN.y, CENTER_BOUNDS_MAX.y),
                position.z
            );
            var enabled = i < numTargets ? (enabledList[i] ? 1.0f : 0.0f) : 0.0f;
            material.SetVector(TargetViewportPositionIDs[i], clampedPosition);
            material.SetFloat(TargetEnabledIDs[i], enabled);
        }
    }

    protected override bool IsDirty => !Mathf.Approximately(_maskThreshold, _lastMaskThreshold);
}
