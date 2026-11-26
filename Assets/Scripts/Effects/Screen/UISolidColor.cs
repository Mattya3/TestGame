using UnityEngine;

public class UISolidColor : MonoUIImageMaterialAccessBehaviour
{
    [SerializeField]
    private UIColor _color;

    private static readonly int ColorID = Shader.PropertyToID("_BaseColor");

    protected override bool IsMaterialValid(Material material)
    {
        if (!material.HasProperty(ColorID))
        {
            Debug.LogError(
                $"Material {material.name} does not have a property named '_BaseColor'."
            );
            return false;
        }
        return true;
    }

    protected override void SetMaterialProperties(Material material)
    {
        material.SetColor(ColorID, _color?.Color ?? Color.black);
    }
}
