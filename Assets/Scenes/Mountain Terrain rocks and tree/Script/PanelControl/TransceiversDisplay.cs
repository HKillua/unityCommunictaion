using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TransceiversDisplay : MonoBehaviour
{
    private CombatUnit combatUnit;
    public GameObject transceiverPanelPrefab;  //显示具体信息的自定义的ui界面，特别注意ui界面的名称一定要符合代码之后的安排

    public Transform transceiversContent;    // 显示ui控件的地方



    void Awake()
    {
        combatUnit = FindObjectOfType<CombatUnit>();
        if (combatUnit != null)
        {
            //Debug.Log("successfully get a combatUnit for display"); 
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
        DisplayTransceivers();
        LayoutRebuilder.ForceRebuildLayoutImmediate(transceiversContent.GetComponent<RectTransform>());
    }


    void DisplayTransceivers()
    {
        foreach (Transform child in transceiversContent)
        {
            Destroy(child.gameObject);
        }

        for(int i= 0; i < combatUnit.transceivers.Count; i++)
        {
            Debug.Log("the number of loops " + i);
            Transceiver transceiver = combatUnit.transceivers[i];
            // 初始化panel 
            GameObject panel = Instantiate(transceiverPanelPrefab, transceiversContent);
            //Toggle toggle = panel.transform.Find("Toggle").GetComponent<Toggle>();
            //toggle.onValueChanged.AddListener((value) => ToggleContent(panel, value));


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

            // 设置Dropdown
            TMP_Dropdown typeDropdown = panel.transform.Find("TypeDropdown").GetComponent<TMP_Dropdown>();
            typeDropdown.ClearOptions();
            List<string> options = new List<string>(Enum.GetNames(typeof(TransceiverType)));
            typeDropdown.AddOptions(options);
            typeDropdown.value = (int)transceiver.type;
            typeDropdown.onValueChanged.AddListener((value) => transceiver.type = (TransceiverType)value);

            // 获取并设置 InputField
            TMP_InputField frequencyInput = panel.transform.Find("FrequencyInput").GetComponent<TMP_InputField>();
            frequencyInput.text = transceiver.frequency.ToString();
            frequencyInput.onEndEdit.AddListener((value) => transceiver.frequency = int.Parse(value));

            TMP_InputField powerInput = panel.transform.Find("PowerInput").GetComponent<TMP_InputField>();
            powerInput.text = transceiver.power.ToString();
            powerInput.onEndEdit.AddListener((value) => transceiver.power = float.Parse(value));

            TMP_InputField sensitivityInput = panel.transform.Find("SensitivityInput").GetComponent<TMP_InputField>();
            sensitivityInput.text = transceiver.sensitivity.ToString();
            sensitivityInput.onEndEdit.AddListener((value) => transceiver.sensitivity = float.Parse(value));

            TMP_InputField modulationInput = panel.transform.Find("ModulationInput").GetComponent<TMP_InputField>();
            modulationInput.text = transceiver.modulation;
            modulationInput.onEndEdit.AddListener((value) => transceiver.modulation = value);

            TMP_InputField bandwidthInput = panel.transform.Find("BandwidthInput").GetComponent<TMP_InputField>();
            bandwidthInput.text = transceiver.bandwidth.ToString();
            bandwidthInput.onEndEdit.AddListener((value) => transceiver.bandwidth = float.Parse(value));


            int index = i ; 
            //panel.transform.Find("DeleteButton").GetComponent<Button>().onClick.AddListener(() => DeleteTransceiver(i));
            Button deleteButton = panel.transform.Find("DeleteButton").GetComponent<Button>();
            deleteButton.onClick.AddListener(() => DeleteTransceiver(index));
            Debug.Log($"Bound delete button event for Element {index}");

            //ToggleContent(panel, false);
        }

    }

         
    void AddTransceiver()
    {
        combatUnit.AddTransceiver(new Transceiver()); 
    }


    void DeleteTransceiver(int index)
    {
        combatUnit.RemoveTransceiver(index);
        Debug.Log("this is index :" + index);
    }


}
