using UnityEngine;

public class PerformanceRestrictor : MonoBehaviour
{
    [SerializeField]
    private int _vSyncCount = 1;

    [SerializeField]
    private int _targetFrameRate = 60;

    void Start()
    {
        QualitySettings.vSyncCount = _vSyncCount;
        Application.targetFrameRate = _targetFrameRate;
    }
}
