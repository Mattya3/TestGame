using UnityEngine;
using UnityEngine.VFX;

public class ElectricBarSparks : MonoBehaviour
{
    [SerializeField]
    private float _spawnPositionYMin = -10.0f;

    [SerializeField]
    private float _spawnPositionYMax = 10.0f;

    [SerializeField]
    private float _spawnIntervalMin = 0.1f;

    [SerializeField]
    private float _spawnIntervalMax = 0.3f;

    [SerializeField]
    private GameObject _lightPrefab;

    [SerializeField]
    private float _lightDuration = 0.1f;

    private VisualEffect _visualEffect;
    private VFXEventAttribute _eventAttribute;

    private float _spawnTimer = 0.0f;

    static readonly string _spawnEventName = "OnSpawn";
    static readonly int _positionYAttribute = Shader.PropertyToID("SparkPositionY");

    private void Awake()
    {
        _visualEffect = GetComponent<VisualEffect>();
        _eventAttribute = _visualEffect.CreateVFXEventAttribute();

        if (!_eventAttribute.HasFloat(_positionYAttribute))
        {
            Debug.LogError("The Visual Effect does not have the required 'SparkPositionY' attribute.");
            enabled = false;
        }
        if (_lightPrefab == null)
        {
            Debug.LogError("Light prefab is not assigned.");
            enabled = false;
        }
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
        float randomY = Random.Range(_spawnPositionYMin, _spawnPositionYMax);
        _eventAttribute.SetFloat(_positionYAttribute, randomY);
        _visualEffect.SendEvent(_spawnEventName, _eventAttribute);

        // light
        var light = Instantiate(_lightPrefab, transform.position + Vector3.up * randomY, Quaternion.identity, transform);
        Destroy(light, _lightDuration);
    }
}
