using System.Collections;
using UnityEngine;

public class LightSourcesPool : MonoBehaviour
{
    [SerializeField]
    private GameObject _lightPrefab;

    [SerializeField, Min(1)]
    private int _poolSize;

    private GameObject[] _pool;
    private Coroutine[] _activeCoroutines;
    private int _lastUsedIndex = -1; // 最後に使用したインデックス

    private void Awake()
    {
        if (_lightPrefab == null)
        {
            Debug.LogError("Light prefab is not assigned.");
            enabled = false;
            return;
        }

        _pool = new GameObject[_poolSize];
        _activeCoroutines = new Coroutine[_poolSize];
        for (int i = 0; i < _poolSize; i++)
        {
            _pool[i] = Instantiate(_lightPrefab);
            _pool[i].SetActive(false);
            _pool[i].transform.parent = transform;
            _activeCoroutines[i] = null;
        }
    }

    public void Spawn(Vector3 position, float duration)
    {
        int index = FindInactiveIndex();

        var light = _pool[index];
        light.transform.position = position;
        light.SetActive(true);

        // 既存のコルーチンがあれば停止
        if (_activeCoroutines[index] != null)
        {
            StopCoroutine(_activeCoroutines[index]);
        }

        _activeCoroutines[index] = StartCoroutine(CoKillInstance(index, duration));
        _lastUsedIndex = index; // 使用したインデックスを記録
    }

    private int FindInactiveIndex()
    {
        // 前回使用したインデックスの次から探索開始
        int startIndex = (_lastUsedIndex + 1) % _poolSize;

        for (int i = 0; i < _poolSize; i++)
        {
            int currentIndex = (startIndex + i) % _poolSize;
            if (!_pool[currentIndex].activeSelf)
                return currentIndex;
        }

        // 全てアクティブな場合は次のインデックスを返す（ラウンドロビン方式）
        return startIndex;
    }

    private IEnumerator CoKillInstance(int index, float duration)
    {
        yield return new WaitForSeconds(duration);
        _pool[index].SetActive(false);
        _activeCoroutines[index] = null;
    }
}
