using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(StageSceneContextReadonlyAccess))]
public class OpeningCameraTarget : MonoEventReactingBehaviour, ICameraTarget
{
    [SerializeField]
    private Vector3 _positionBegin;

    [SerializeField]
    private Vector3 _positionEnd;

    [SerializeField]
    private float _progressFactor = 0f; // 0から1の範囲で、Animatorを使って進行度を制御するための値

    private StageSceneContextReadonlyAccess _stageSceneContextAccess;
    private bool _stillInOpeningAnimation = true;

    private void Awake()
    {
        _stageSceneContextAccess = GetComponent<StageSceneContextReadonlyAccess>();
    }

    public void OnStart()
    {
    }

    public bool IsActive
    {
        get
        {
            if (_stageSceneContextAccess.AfterRestart)
                return false; // 再スタート後はこのカメラターゲットは無効

            return _stillInOpeningAnimation; // 開始アニメーション終了後は無効
        }
    }

    public Vector3 Position
    {
        get
        {
            // Animatorの進行度に基づいて、開始位置と終了位置の間を線形補間する
            float progress = Mathf.Clamp01(_progressFactor);
            return Vector3.Lerp(_positionBegin, _positionEnd, progress);
        }
    }

    public bool EnableCollider => false; // カメラのコライダーは無効

    protected override void OnPlayStart()
    {
        // ステージ開始時にカメラターゲットを終了
        _stillInOpeningAnimation = false;
    }
}
