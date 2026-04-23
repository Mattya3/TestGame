using System.Collections;
using UnityEngine;

public class LightSourcesPool : MonoBehaviour
{
    [SerializeField]
    private GameObject _lightPrefab;

    [SerializeField, Min(1)]
    private int _poolSize;

    private GameObject[] _pool;
    private int[] _spawnIndices;
    private int _spawnCount = 0;

    private void Awake()
    {
        if (_lightPrefab == null)
        {
            Debug.LogError("Light prefab is not assigned.");
            enabled = false;
            return;
        }

        _pool = new GameObject[_poolSize];
        _spawnIndices = new int[_poolSize];
        for (int i = 0; i < _poolSize; i++)
        {
            _pool[i] = Instantiate(_lightPrefab);
            _pool[i].SetActive(false);
            _pool[i].transform.parent = transform; // プールオブジェクトの子にする
            _spawnIndices[i] = -1;
        }
    }

    public void Spawn(Vector3 position, float duration)
    {
        var nextLocation = _spawnCount % _poolSize;

        var light = _pool[nextLocation];
        light.transform.position = position;
        light.SetActive(true);
        _spawnIndices[nextLocation] = _spawnCount;
        StartCoroutine(_CoKillInstance(light, duration, _spawnCount));
        
        _spawnCount++;
    }

    private IEnumerator _CoKillInstance(GameObject light, float duration, int index)
    {
        yield return new WaitForSeconds(duration);
        if (_spawnIndices[index % _poolSize] != index)
            yield break; // すでに別のインスタンスが上書きされている場合は何もしない
        light.SetActive(false);
    }
}
