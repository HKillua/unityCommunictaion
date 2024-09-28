using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MaterialSelector : MonoBehaviour
{
    public Material[] materials; // 存储多个材质
    private Renderer objRenderer;

    [SerializeField]
    private int selectedMaterialIndex = 0; // 默认选择的材质索引

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        ApplyMaterial();
    }

    void ApplyMaterial()
    {
        if (materials != null && materials.Length > 0)
        {
            selectedMaterialIndex = Mathf.Clamp(selectedMaterialIndex, 0, materials.Length - 1);
            objRenderer.material = materials[selectedMaterialIndex];
        }
    }

    // 在 Inspector 中选择材质时调用此方法
    public void SetMaterial(int index)
    {
        selectedMaterialIndex = Mathf.Clamp(index, 0, materials.Length - 1);
        ApplyMaterial();
    }

    // 为了在 Inspector 中实时更新材质
    void OnValidate()
    {
        if (objRenderer == null)
        {
            objRenderer = GetComponent<Renderer>();
        }
        ApplyMaterial();
    }
}


