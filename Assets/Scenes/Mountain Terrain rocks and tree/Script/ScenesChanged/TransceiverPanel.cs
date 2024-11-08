using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransceiverPanel : MonoBehaviour
{
    private CombatUnit combatunit;              // 需要显示的是哪一个CombatUnit当中的内容
    public Transform transceiversContent;       // panel需要显示的地方
    public GameObject panelPrefab;              //需要显示的panel的预制件


    public void Bind(CombatUnit _combatunit)
    {
        combatunit = _combatunit;
        // 当绑定的panel不为空的时候就选择，通过Transceier List里面的数据进行刷新显示。
        if (combatunit != null)
        {
            UpdateUI(combatunit); 
        }
        else
        {
            ClearDisplay(transceiversContent);  // 此处的逻辑应该是清除掉选择框中的所有数据
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

    // 显示对应的信息并且增加修改的监听事件
    void DisplayInformation(GameObject panel , Transceiver transceiver, int i)
    {
        // 设置elementname显示出来
        panel.transform.Find("ElementName").GetComponent<TMP_Text>().text = $"Transceiver {i}";

        // 设置typeDropdown组件
        TMP_Dropdown typeDropdown = panel.transform.Find("TypeDropdown").GetComponent<TMP_Dropdown>();
        typeDropdown.ClearOptions();
        List<string> options = new List<string>(Enum.GetNames(typeof(TransceiverType)));
        typeDropdown.AddOptions(options);
        typeDropdown.value = (int)transceiver.type;   // 设置typeDown组件会显示的类型
        typeDropdown.onValueChanged.AddListener((value) => transceiver.type = (TransceiverType)value);


        // 设置InputFiled显示
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


        // 将删除按钮和对应的元素进行顺序绑定
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
   
            Transceiver transceiver = _combatunit.transceivers[i];   // 获得到接收机的信息
            // 将transceiver和panel上的组件关联起来，方便做到数据的修改
            DisplayInformation(panel , transceiver ,i);    
            // 此处可以添加后续的删除逻辑

           
        }
        
    }

    private void DeleteTransceiver(int index)
    {
        if (combatunit != null)
        {
            combatunit.RemoveTransceiver(index);
            // 删除之后要重新刷新显示UI 
            UpdateUI(combatunit); 
        }
        
    }
    
}
