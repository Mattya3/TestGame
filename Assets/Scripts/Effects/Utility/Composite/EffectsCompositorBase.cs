using EffectsCompositeComponent;
using UnityEngine;
using UnityEngine.VFX;

public abstract class EffectsCompositorBase : MonoBehaviour, IEffectsCompositor
{
    [SerializeField]
    [ShowIf(nameof(IsRoot))]
    private bool _playInUnscaledTime = false;

    private IEffectsCompositor _parentCompositor;

    private VisualEffect _visualEffect;
    private ISoundEffect[] _soundEffects;
    private ILightSourceEffect[] _lightSourceEffects;
    private ICameraEffect[] _cameraEffects;
    private ITransformEffect[] _transformEffects;
    private IRendererEffect[] _rendererEffects;
    private IInstantiationEffect[] _instantiationEffects;

    // ルートオブジェクトかどうかを判定するプロパティ
    // 親がいない場合はルート
    // あるいはいたとしても、IEffectsCompositorを実装したコンポーネントが親に存在しない場合もルートとみなす
    public bool IsRoot
    {
        get
        {
            if ( transform.parent == null)
                return true;

            // エディタの場合はGetComponentInParentで検索
            if (Application.isEditor)
            {
                var parentCompositor = transform.parent.GetComponentInParent<IEffectsCompositor>();
                return parentCompositor == null;
            }
            // 実行時は_parentCompositorで判定
            else
            {
                return _parentCompositor == null;
            }
        }
    }

    protected virtual void Awake()
    {
        _parentCompositor = transform.parent?.GetComponentInParent<IEffectsCompositor>();

        _visualEffect = GetComponent<VisualEffect>();
        _soundEffects = GetComponents<ISoundEffect>();
        _lightSourceEffects = GetComponents<ILightSourceEffect>();
        _cameraEffects = GetComponents<ICameraEffect>();
        _transformEffects = GetComponents<ITransformEffect>();
        _rendererEffects = GetComponents<IRendererEffect>();
        _instantiationEffects = GetComponents<IInstantiationEffect>();
    }

    protected void InitializeComponents(
        AudioSource audioSource,
        CameraMutableAccess cameraAccess,
        TransformOffsetController transformOffsetController,
        Renderer renderer,
        Transform instantiationParent
    )
    {
        if (_visualEffect != null)
            _visualEffect.pause = true;

        foreach (var soundEffect in _soundEffects)
        {
            soundEffect.Initialize(audioSource);
        }
        foreach (var lightSourceEffect in _lightSourceEffects)
        {
            lightSourceEffect.Initialize(PlayInUnscaledTime);
        }
        foreach (var cameraEffect in _cameraEffects)
        {
            cameraEffect.Initialize(cameraAccess, PlayInUnscaledTime);
        }
        foreach (var transformEffect in _transformEffects)
        {
            transformEffect.Initialize(transformOffsetController, PlayInUnscaledTime);
        }
        foreach (var rendererEffect in _rendererEffects)
        {
            rendererEffect.Initialize(renderer, PlayInUnscaledTime);
        }
        foreach (var instantiationEffect in _instantiationEffects)
        {
            instantiationEffect.Initialize(instantiationParent, PlayInUnscaledTime);
        }
    }

    protected void PlayComponents()
    {
        if (_visualEffect != null)
        {
            _visualEffect.Play();
        }
        foreach (var soundEffect in _soundEffects)
        {
            if (soundEffect.isEnabled)
                soundEffect.Play();
        }
        foreach (var lightSourceEffect in _lightSourceEffects)
        {
            if (lightSourceEffect.isEnabled)
                lightSourceEffect.Play();
        }
        foreach (var cameraEffect in _cameraEffects)
        {
            if (cameraEffect.isEnabled)
                cameraEffect.Play();
        }
        foreach (var transformEffect in _transformEffects)
        {
            if (transformEffect.isEnabled)
                transformEffect.Play();
        }
        foreach (var rendererEffect in _rendererEffects)
        {
            if (rendererEffect.isEnabled)
                rendererEffect.Play();
        }
        foreach (var instantiationEffect in _instantiationEffects)
        {
            if (instantiationEffect.isEnabled)
                instantiationEffect.Play();
        }
    }

    protected virtual void Update()
    {
        if (_visualEffect == null)
            return;

        // 時間スケールに対応するため、手動で更新
        _visualEffect.Simulate(PlayInUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
    }

    // 親のEffectsCompositorBaseが存在する場合は、親の設定を優先
    public bool PlayInUnscaledTime => _parentCompositor != null ? _parentCompositor.PlayInUnscaledTime : _playInUnscaledTime;

    public abstract void Initialize(AudioSource audioSource, CameraMutableAccess cameraAccess, TransformOffsetController transformOffsetController, Renderer renderer, Transform instantiationParent);
    public abstract void PlayEffects();
    public abstract void StopEffects();
}
