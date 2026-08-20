using UnityEngine;

[RequireComponent(typeof(CameraReadonlyAccess))]
public class HoleMaskController : MonoUIImageMaterialAccessBehaviour
{
    private const int MAX_NUM_TARGETS = 2;

    [SerializeField]
    private float _maskThreshold = 0.0f;

    private float _lastMaskThreshold = float.MaxValue;
    private CameraReadonlyAccess _cameraAccess;
    private IHoleMaskTarget _target;

    private static readonly int MaskThresholdID = Shader.PropertyToID("_MaskThreshold");
    private static readonly int CameraAspectID = Shader.PropertyToID("_CameraAspect");
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
        for (int i = 0; i < MAX_NUM_TARGETS; i++)
        {
            if (!material.HasProperty(TargetViewportPositionIDs[i]))
            {
                Debug.LogError(
                    $"Material {material.name} does not have a property named '{TargetViewportPositionIDs[i]}'."
                );
                return false;
            }
            if (!material.HasProperty(TargetEnabledIDs[i]))
            {
                Debug.LogError(
                    $"Material {material.name} does not have a property named '{TargetEnabledIDs[i]}'."
                );
                return false;
            }
        }
        return true;
    }

    protected override void SetMaterialProperties(Material material)
    {
        material.SetFloat(MaskThresholdID, _maskThreshold);
        _lastMaskThreshold = _maskThreshold;

        // エディタ上ではオブジェクトを参照せずデフォルトの値を設定
        if (!Application.isPlaying)
            return;

        material.SetFloat(CameraAspectID, _cameraAccess.AspectRatio);

        var enabledList = _target.AreEnabled;
        var screenPositionsList = _target.ViewportPositions;
        var numTargets = Mathf.Min(enabledList.Count, screenPositionsList.Count, MAX_NUM_TARGETS);
        for (int i = 0; i < MAX_NUM_TARGETS; i++)
        {
            var position = i < numTargets ? screenPositionsList[i] : Vector3.zero;
            var enabled = i < numTargets ? (enabledList[i] ? 1.0f : 0.0f) : 0.0f;
            material.SetVector(TargetViewportPositionIDs[i], position);
            material.SetFloat(TargetEnabledIDs[i], enabled);
        }
    }

    protected override bool IsDirty => !Mathf.Approximately(_maskThreshold, _lastMaskThreshold);
}
