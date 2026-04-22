using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class CustomTimeLooper : MonoBehaviour
{
    [SerializeField, Min(1e-3f)]
    private float _loopDuration = 1f;

    private Material _material;
    private float _time = 0f;

    private const string TIME_PROPERTY_NAME = "_CustomTime";

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
        if (_material == null)
        {
            Debug.LogError("Renderer does not have a material.", this);
            enabled = false;
            return;
        }
    }

    private void OnDestroy()
    {
        // 明示的にマテリアルを破棄して、メモリリークを防止
        if (_material != null)
        {
            Destroy(_material);
            _material = null;
        }
    }

    void Update()
    {
        _time += Time.deltaTime;
        if (_time >= _loopDuration)
            _time -= _loopDuration;

        _material.SetFloat(TIME_PROPERTY_NAME, _time / _loopDuration);
    }
}
