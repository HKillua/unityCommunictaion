using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AntennaEditor : MonoBehaviour
{
    [SerializeField] private TMP_InputField antennaTypeInput;
    [SerializeField] private TMP_InputField antennaGainInput;
    [SerializeField] private TMP_InputField antennaDirectionXInput;
    [SerializeField] private TMP_InputField antennaDirectionYInput;
    [SerializeField] private TMP_InputField antennaDirectionZInput;
    [SerializeField] private TMP_InputField antennaBeamWidthInput;
    [SerializeField] private TMP_InputField antennaPolarizationInput;

    private Antenna antenna;
    private CombatUnit combatUnit;

    void Awake()
    {
        combatUnit = FindObjectOfType<CombatUnit>();
        Debug.Log("CombatUnit For AntennaEditor loaded");
    }

    void Start()
    {
        antennaTypeInput.onEndEdit.AddListener(UpdateAntennaType);
        antennaGainInput.onEndEdit.AddListener(UpdateAntennaGain);
        antennaDirectionXInput.onEndEdit.AddListener(UpdateAntennaDirectionX);
        antennaDirectionYInput.onEndEdit.AddListener(UpdateAntennaDirectionY);
        antennaDirectionZInput.onEndEdit.AddListener(UpdateAntennaDirectionZ);
        antennaBeamWidthInput.onEndEdit.AddListener(UpdateAntennaBeamWidth);
        antennaPolarizationInput.onEndEdit.AddListener(UpdateAntennaPolarization);

        antenna = new Antenna();
    }

    public void UpdateAntennaType(string newType)
    {
        antenna.type = newType;
    }

    public void UpdateAntennaGain(string newGain)
    {
        if (float.TryParse(newGain, out float gain))
        {
            antenna.gain = gain;
        }
    }

    public void UpdateAntennaDirectionX(string newX)
    {
        if (float.TryParse(newX, out float x))
        {
            antenna.direction.x = x;
        }
    }

    public void UpdateAntennaDirectionY(string newY)
    {
        if (float.TryParse(newY, out float y))
        {
            antenna.direction.y = y;
        }
    }

    public void UpdateAntennaDirectionZ(string newZ)
    {
        if (float.TryParse(newZ, out float z))
        {
            antenna.direction.z = z;
        }
    }

    public void UpdateAntennaBeamWidth(string newBeamWidth)
    {
        if (float.TryParse(newBeamWidth, out float beamWidth))
        {
            antenna.beamWidth = beamWidth;
        }
    }

    public void UpdateAntennaPolarization(string newPolarization)
    {
        antenna.polarization = newPolarization;
    }

    // ���������޸ĺ�� Antenna ��ӵ� CombatUnit ���б���
    public void SaveAntenna()
    {
        // �ֶ����� onEndEdit �¼���ȷ�����������ֶε�ֵ�������µ� antenna ������
        antennaTypeInput.onEndEdit.Invoke(antennaTypeInput.text);
        antennaGainInput.onEndEdit.Invoke(antennaGainInput.text);
        antennaDirectionXInput.onEndEdit.Invoke(antennaDirectionXInput.text);
        antennaDirectionYInput.onEndEdit.Invoke(antennaDirectionYInput.text);
        antennaDirectionZInput.onEndEdit.Invoke(antennaDirectionZInput.text);
        antennaBeamWidthInput.onEndEdit.Invoke(antennaBeamWidthInput.text);
        antennaPolarizationInput.onEndEdit.Invoke(antennaPolarizationInput.text);

        combatUnit.AddAntenna(antenna);
        antenna = new Antenna(); // ���³�ʼ���Ա��´�����

        Debug.Log("Antenna saved: " + antenna.type);
    }
}
