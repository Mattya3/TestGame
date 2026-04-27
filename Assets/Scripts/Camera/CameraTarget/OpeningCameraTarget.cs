using UnityEngine;

[RequireComponent(typeof(Animator))]
public class OpeningCameraTarget : MonoBehaviour, ICameraTarget
{
    [SerializeField]
    private Vector3 _positionBegin;

    [SerializeField]
    private Vector3 _positionEnd;

    [SerializeField]
    private float _progressFactor = 0f; // 0から1の範囲で、Animatorを使って進行度を制御するための値

    public void OnStart()
    {
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
}
