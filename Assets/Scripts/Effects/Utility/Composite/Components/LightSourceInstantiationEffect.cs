using EffectsCompositeComponent;
using UnityEngine;

namespace EffectsCompositeComponents
{
    public class LightSourceInstantiationEffect : MonoBehaviour, IInstantiationEffect
    {
        [SerializeField]
        private GameObject _lightPrefab;

        [SerializeField, Min(1e-3f)]
        private float _duration = 1.0f;

        [SerializeField]
        private bool _independentInstances = false;

        [SerializeField, Min(1)]
        private int _poolSize = 5;

        private LightSourcesPool _lightSourcesPool;

        public bool isEnabled => enabled;

        public void Initialize(Transform instantiationParent, bool playInUnscaledTime)
        {
            _lightSourcesPool = new LightSourcesPool(this, _lightPrefab, _poolSize, playInUnscaledTime, _independentInstances ? null : instantiationParent);
        }

        public void Play()
        {
            _lightSourcesPool.Spawn(transform.position, _duration);
        }
    }
}