using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
public class TransceiverEditor : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown transceiverTypeDropdown;
    [SerializeField] private TMP_InputField transceiverFrequencyInput;
    [SerializeField] private TMP_InputField transceiverPowerInput;
    [SerializeField] private TMP_InputField transceiverSensitivityInput;
    [SerializeField] private TMP_InputField transceiverModulationInput;
    [SerializeField] private TMP_InputField transceiverBandwidthInput;

    
    private Transceiver transceiver;
    private Antenna antenna;
    private CombatUnit combatunit;

  
    void Awake()
    {
        combatunit = FindObjectOfType<CombatUnit>();
        Debug.Log("combatunit loading");
    }
    void Start()
    {
        InitializeTypeDropdown();

        transceiverFrequencyInput.onEndEdit.AddListener(UpdateTransceiverFrequency);
        transceiverPowerInput.onEndEdit.AddListener(UpdateTransceiverPower);
        transceiverSensitivityInput.onEndEdit.AddListener(UpdateTransceiverSensitivity);
        transceiverModulationInput.onEndEdit.AddListener(UpdateTransceiverModulation);
        transceiverBandwidthInput.onEndEdit.AddListener(UpdateTransceiverBandwidth);

        transceiverTypeDropdown.onValueChanged.AddListener(UpdateTransceiverType);

        transceiver = new Transceiver();
        //antenna = new Antenna();

      
        //combatunit = FindObjectOfType<CombatUnit>();

    }
    public void UpdateTransceiverFrequency(string newFrequency)
    {
        if (int.TryParse(newFrequency, out int frequency))
        {
            transceiver.frequency = frequency;
        }
    }

    public void UpdateTransceiverPower(string newPower)
    {
        if (float.TryParse(newPower, out float power))
        {
            transceiver.power = power;
        }
    }

    public void UpdateTransceiverSensitivity(string newSensitivity)
    {
        if (float.TryParse(newSensitivity, out float sensitivity))
        {
            transceiver.sensitivity = sensitivity;
        }
    }

    public void UpdateTransceiverModulation(string newModulation)
    {
        transceiver.modulation = newModulation;
    }

    public void UpdateTransceiverBandwidth(string newBandwidth)
    {
        if (float.TryParse(newBandwidth, out float bandwidth))
        {
            transceiver.bandwidth = bandwidth;
        }
    }

    public void UpdateTransceiverType(int newType)
    {
        transceiver.type = (TransceiverType)newType;
    }

    private void InitializeTypeDropdown()
    {
        transceiverTypeDropdown.ClearOptions();
        List<string> options = new List<string>(System.Enum.GetNames(typeof(TransceiverType)));
        transceiverTypeDropdown.AddOptions(options);
    }

    // ���������޸ĺ�� Transceiver ��ӵ� ModelManager ���б���
    public void SaveTransceiver()
    {
        // �ֶ����� onEndEdit �¼���ȷ�����������ֶε�ֵ�������µ� transceiver ������
        transceiverFrequencyInput.onEndEdit.Invoke(transceiverFrequencyInput.text);
        transceiverPowerInput.onEndEdit.Invoke(transceiverPowerInput.text);
        transceiverSensitivityInput.onEndEdit.Invoke(transceiverSensitivityInput.text);
        transceiverModulationInput.onEndEdit.Invoke(transceiverModulationInput.text);
        transceiverBandwidthInput.onEndEdit.Invoke(transceiverBandwidthInput.text);


        combatunit.AddTransceiver(transceiver);
        transceiver = new Transceiver(); // ���³�ʼ���Ա��´�����

    }



}
