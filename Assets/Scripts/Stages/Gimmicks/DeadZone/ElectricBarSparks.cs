using UnityEngine;
using UnityEngine.VFX;

public class ElectricBarSparks : MonoBehaviour
{
    [SerializeField]
    private GameObject _effectPrefab;

    [SerializeField]
    private float _spawnPositionMin = -10.0f;

    [SerializeField]
    private float _spawnPositionMax = 10.0f;

    [SerializeField]
    private float _spawnIntervalMin = 0.1f;

    [SerializeField]
    private float _spawnIntervalMax = 0.3f;

    [SerializeField]
    private AudioSource _audioSource;

    private EffectsCompositePlayer _effectsPlayer;

    private float _spawnTimer = 0.0f;

    private void Awake()
    {
        if (_effectPrefab == null)
        {
            Debug.LogError("Effect prefab is not assigned.", this);
            enabled = false;
            return;
        }
        _effectsPlayer = new EffectsCompositePlayer(_effectPrefab, _audioSource, null, null, null, transform.position, transform.rotation, transform);
    }

    private void OnDestroy()
    {
        _effectsPlayer?.Cleanup();
    }

    private void Start()
    {
        _ResetSpawnTimer();
    }

    private void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer > 0.0f)
            return;

        _SpawnSpark();
        _ResetSpawnTimer();
    }

    private void _ResetSpawnTimer()
    {
        _spawnTimer = Random.Range(_spawnIntervalMin, _spawnIntervalMax);
    }

    private void _SpawnSpark()
    {
        float randomPos = Random.Range(_spawnPositionMin, _spawnPositionMax);
        Vector3 spawnPos = transform.position + transform.up * randomPos;
        _effectsPlayer?.PlayEffects(spawnPos);
    }
}
