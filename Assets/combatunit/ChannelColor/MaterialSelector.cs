using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MaterialSelector : MonoBehaviour
{
    public Material[] materials; // �洢�������
    private Renderer objRenderer;

    [SerializeField]
    private int selectedMaterialIndex = 0; // Ĭ��ѡ��Ĳ�������

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

    // �� Inspector ��ѡ�����ʱ���ô˷���
    public void SetMaterial(int index)
    {
        selectedMaterialIndex = Mathf.Clamp(index, 0, materials.Length - 1);
        ApplyMaterial();
    }

    // Ϊ���� Inspector ��ʵʱ���²���
    void OnValidate()
    {
        if (objRenderer == null)
        {
            objRenderer = GetComponent<Renderer>();
        }
        ApplyMaterial();
    }
}


