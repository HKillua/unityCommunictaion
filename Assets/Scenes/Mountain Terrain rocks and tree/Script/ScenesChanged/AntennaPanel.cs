using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; 

public class AntennaPanel : MonoBehaviour
{
    private CombatUnit combatunit;

    public Transform AntennaContent;
    public GameObject panelPrefab; 

    public void Bind(CombatUnit _combatunit)
    {
        combatunit = _combatunit;

        if (combatunit != null)
        {
            UpdateUI(combatunit);
        }
        else
        {
           ClearDisplay(AntennaContent); 
        }
    }

    void UpdateUI(CombatUnit _combatunit)
    {
        ClearDisplay(AntennaContent); 
        DisplayAntenna(_combatunit);
    }

    void ClearDisplay(Transform content)
    {
        foreach(Transform trans in content)
        {
            Destroy(trans.gameObject);
        }
    }

    void DisplayAntenna(CombatUnit _combatunit)
    {
        for (int i = 0; i < _combatunit.antennas.Count; i++)
        {
            Debug.Log("i will display the content of antenna");
            GameObject panel = Instantiate(panelPrefab, AntennaContent);
            Antenna antenna = _combatunit.antennas[i];

            DisplayInformation(panel, antenna, i); 

        }
    }

    void DisplayInformation(GameObject panel , Antenna antenna , int i)
    {
        // 设置elementname显示出来
        panel.transform.Find("ElementName").GetComponent<TMP_Text>().text = $"Antenna {i}";
        
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
    }

    private void DeleteAntenna(int index)
    {
        if (combatunit != null)
        {
            combatunit.RemoveAntenna(index);
            UpdateUI(combatunit);   // 刷新显示
        }
    }


}
