using UnityEngine;
using UnityEngine.VFX;

public class EffectsCompositor : MonoBehaviour
{
    [SerializeField]
    private float _duration = 1f;

    [SerializeField]
    private float _delayTime = 0f;

    private VisualEffect _visualEffect;

    private void Awake()
    {
        _visualEffect = GetComponent<VisualEffect>();
    }

    public void Initialize()
    {

    }

    public void PlayEffects()
    {
        _visualEffect.Play();
    }

    private void Update()
    {
        
    }
}
