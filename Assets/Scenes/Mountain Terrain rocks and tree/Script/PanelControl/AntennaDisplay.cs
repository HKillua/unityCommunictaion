using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AntennaDisplay : MonoBehaviour
{
    private CombatUnit combatUnit;
    public GameObject antennaPanelPrefab;  // 显示具体信息的自定义的UI界面
    public Transform antennasContent;      // 显示UI控件的地方

    void Awake()
    {
        combatUnit = FindObjectOfType<CombatUnit>();
        if (combatUnit != null)
        {
            Debug.Log("Successfully got a CombatUnit for display");
        }
    }

    private void OnEnable()
    {
        combatUnit.OnDataChanged += UpdateUI;
        UpdateUI();
    }

    private void OnDisable()
    {
        combatUnit.OnDataChanged -= UpdateUI;
    }

    void UpdateUI()
    {
        DisplayAntennas();
        LayoutRebuilder.ForceRebuildLayoutImmediate(antennasContent.GetComponent<RectTransform>());
    }

    void DisplayAntennas()
    {
        foreach (Transform child in antennasContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < combatUnit.antennas.Count; i++)
        {
            Debug.Log("The number of loops " + i);
            Antenna antenna = combatUnit.antennas[i];
            // 初始化panel 
            GameObject panel = Instantiate(antennaPanelPrefab, antennasContent);

            // 设置panel的RectTransform属性
            RectTransform rectTransform = panel.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.localScale = Vector3.one; // 确保缩放为1
                rectTransform.anchorMin = new Vector2(0, 1); // 锚点在父对象的顶部
                rectTransform.anchorMax = new Vector2(1, 1);
                rectTransform.pivot = new Vector2(0.5f, 1);
                rectTransform.anchoredPosition = new Vector2(0, -i * rectTransform.sizeDelta.y); // 按顺序垂直排列
            }

            // 设置elementname显示出来
            panel.transform.Find("ElementName").GetComponent<TMP_Text>().text = $"Element {i}";

            // 获取并设置 InputField
            TMP_InputField typeInput = panel.transform.Find("TypeInput").GetComponent<TMP_InputField>();
            typeInput.text = antenna.type;
            typeInput.onEndEdit.AddListener((value) => antenna.type = value);

            TMP_InputField gainInput = panel.transform.Find("GainInput").GetComponent<TMP_InputField>();
            gainInput.text = antenna.gain.ToString();
            gainInput.onEndEdit.AddListener((value) => antenna.gain = float.Parse(value));

            TMP_InputField directionXInput = panel.transform.Find("DirectionXInput").GetComponent<TMP_InputField>();
            directionXInput.text = antenna.direction.x.ToString();
            directionXInput.onEndEdit.AddListener((value) => antenna.direction.x = float.Parse(value));

            TMP_InputField directionYInput = panel.transform.Find("DirectionYInput").GetComponent<TMP_InputField>();
            directionYInput.text = antenna.direction.y.ToString();
            directionYInput.onEndEdit.AddListener((value) => antenna.direction.y = float.Parse(value));

            TMP_InputField directionZInput = panel.transform.Find("DirectionZInput").GetComponent<TMP_InputField>();
            directionZInput.text = antenna.direction.z.ToString();
            directionZInput.onEndEdit.AddListener((value) => antenna.direction.z = float.Parse(value));

            TMP_InputField beamWidthInput = panel.transform.Find("BeamWidthInput").GetComponent<TMP_InputField>();
            beamWidthInput.text = antenna.beamWidth.ToString();
            beamWidthInput.onEndEdit.AddListener((value) => antenna.beamWidth = float.Parse(value));

            TMP_InputField polarizationInput = panel.transform.Find("PolarizationInput").GetComponent<TMP_InputField>();
            polarizationInput.text = antenna.polarization;
            polarizationInput.onEndEdit.AddListener((value) => antenna.polarization = value);

            int index = i;
            Button deleteButton = panel.transform.Find("DeleteButton").GetComponent<Button>();
            deleteButton.onClick.AddListener(() => DeleteAntenna(index));
            Debug.Log($"Bound delete button event for Element {index}");
        }
    }

    void AddAntenna()
    {
        combatUnit.AddAntenna(new Antenna());
    }

    void DeleteAntenna(int index)
    {
        combatUnit.RemoveAntenna(index);
        Debug.Log("This is index :" + index);
    }
}
