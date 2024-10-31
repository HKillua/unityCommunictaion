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
    public GameObject transceiverPanelPrefab;  //��ʾ������Ϣ���Զ����ui���棬�ر�ע��ui���������һ��Ҫ���ϴ���֮��İ���

    public Transform transceiversContent;    // ��ʾui�ؼ��ĵط�



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
            // ��ʼ��panel 
            GameObject panel = Instantiate(transceiverPanelPrefab, transceiversContent);
            //Toggle toggle = panel.transform.Find("Toggle").GetComponent<Toggle>();
            //toggle.onValueChanged.AddListener((value) => ToggleContent(panel, value));


            // ����panel��RectTransform����
            RectTransform rectTransform = panel.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.localScale = Vector3.one; // ȷ������Ϊ1
                rectTransform.anchorMin = new Vector2(0, 1); // ê���ڸ�����Ķ���
                rectTransform.anchorMax = new Vector2(1, 1);
                rectTransform.pivot = new Vector2(0.5f, 1);
                rectTransform.anchoredPosition = new Vector2(0, -i * rectTransform.sizeDelta.y); // ��˳��ֱ����
            }

            // ����elementname��ʾ����
            panel.transform.Find("ElementName").GetComponent<TMP_Text>().text = $"Element {i}";

            // ����Dropdown
            TMP_Dropdown typeDropdown = panel.transform.Find("TypeDropdown").GetComponent<TMP_Dropdown>();
            typeDropdown.ClearOptions();
            List<string> options = new List<string>(Enum.GetNames(typeof(TransceiverType)));
            typeDropdown.AddOptions(options);
            typeDropdown.value = (int)transceiver.type;
            typeDropdown.onValueChanged.AddListener((value) => transceiver.type = (TransceiverType)value);

            // ��ȡ������ InputField
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
