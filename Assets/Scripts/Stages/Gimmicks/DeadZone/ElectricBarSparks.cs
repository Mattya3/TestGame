using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
[RequireComponent(typeof(LightSourcesPool))]
public class ElectricBarSparks : MonoBehaviour
{
    [SerializeField]
    private float _spawnPositionMin = -10.0f;

    [SerializeField]
    private float _spawnPositionMax = 10.0f;

    [SerializeField]
    private float _spawnIntervalMin = 0.1f;

    [SerializeField]
    private float _spawnIntervalMax = 0.3f;

    [SerializeField]
    private float _lightDuration = 0.1f;

    private VisualEffect _visualEffect;
    private VFXEventAttribute _eventAttribute;
    private LightSourcesPool _lightPool;

    private float _spawnTimer = 0.0f;

    static readonly string _spawnEventName = "OnSpawn";
    static readonly int _positionAttribute = Shader.PropertyToID("SparkPosition");

    private void Awake()
    {
        _visualEffect = GetComponent<VisualEffect>();
        _eventAttribute = _visualEffect.CreateVFXEventAttribute();
        _lightPool = GetComponent<LightSourcesPool>();

        if (!_eventAttribute.HasFloat(_positionAttribute))
        {
            Debug.LogError(
                "The Visual Effect does not have the required 'SparkPosition' attribute."
            );
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
        float randomPos = Random.Range(_spawnPositionMin, _spawnPositionMax);
        _eventAttribute.SetFloat(_positionAttribute, randomPos);
        _visualEffect.SendEvent(_spawnEventName, _eventAttribute);
        _lightPool.Spawn(transform.position + transform.up * randomPos, _lightDuration);
    }
}
