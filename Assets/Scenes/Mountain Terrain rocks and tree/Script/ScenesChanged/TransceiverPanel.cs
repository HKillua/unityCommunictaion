using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransceiverPanel : MonoBehaviour
{
    private CombatUnit combatunit;              // ��Ҫ��ʾ������һ��CombatUnit���е�����
    public Transform transceiversContent;       // panel��Ҫ��ʾ�ĵط�
    public GameObject panelPrefab;              //��Ҫ��ʾ��panel��Ԥ�Ƽ�


    public void Bind(CombatUnit _combatunit)
    {
        combatunit = _combatunit;
        // ���󶨵�panel��Ϊ�յ�ʱ���ѡ��ͨ��Transceier List��������ݽ���ˢ����ʾ��
        if (combatunit != null)
        {
            UpdateUI(combatunit); 
        }
        else
        {
            ClearDisplay(transceiversContent);  // �˴����߼�Ӧ���������ѡ����е���������
        }

    }

    void UpdateUI(CombatUnit _combatunit)
    {
        ClearDisplay(transceiversContent); 
        DisplayTransceivers(_combatunit);
    }

    void ClearDisplay(Transform content)
    {
        foreach(Transform trans in content)
        {
            Destroy(trans.gameObject);
        }
    }

    // ��ʾ��Ӧ����Ϣ���������޸ĵļ����¼�
    void DisplayInformation(GameObject panel , Transceiver transceiver, int i)
    {
        // ����elementname��ʾ����
        panel.transform.Find("ElementName").GetComponent<TMP_Text>().text = $"Transceiver {i}";

        // ����typeDropdown���
        TMP_Dropdown typeDropdown = panel.transform.Find("TypeDropdown").GetComponent<TMP_Dropdown>();
        typeDropdown.ClearOptions();
        List<string> options = new List<string>(Enum.GetNames(typeof(TransceiverType)));
        typeDropdown.AddOptions(options);
        typeDropdown.value = (int)transceiver.type;   // ����typeDown�������ʾ������
        typeDropdown.onValueChanged.AddListener((value) => transceiver.type = (TransceiverType)value);


        // ����InputFiled��ʾ
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


        // ��ɾ����ť�Ͷ�Ӧ��Ԫ�ؽ���˳���
        int index = i;
        Button deleteButton = panel.transform.Find("DeleteButton").GetComponent<Button>();
        deleteButton.onClick.AddListener(() => DeleteTransceiver(index));

    }


    void DisplayTransceivers(CombatUnit _combatunit )
    { 

        for (int i = 0; i < _combatunit.transceivers.Count; i++)
        {
            Debug.Log("i will display the content of transceivers");
            GameObject panel = Instantiate(panelPrefab, transceiversContent); 
   
            Transceiver transceiver = _combatunit.transceivers[i];   // ��õ����ջ�����Ϣ
            // ��transceiver��panel�ϵ�������������������������ݵ��޸�
            DisplayInformation(panel , transceiver ,i);    
            // �˴�������Ӻ�����ɾ���߼�

           
        }
        
    }

    private void DeleteTransceiver(int index)
    {
        if (combatunit != null)
        {
            combatunit.RemoveTransceiver(index);
            // ɾ��֮��Ҫ����ˢ����ʾUI 
            UpdateUI(combatunit); 
        }
        
    }
    
}
