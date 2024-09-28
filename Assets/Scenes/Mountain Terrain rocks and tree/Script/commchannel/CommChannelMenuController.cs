using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;


// �˽ű�Ӧ��Ҫ���ڵ����ť����ֵĴ��ڽ�����     
public class CommChannelMenuController : MonoBehaviour
{
    // ��ui��������Ķ�Ӧ�������
    
    public TMP_Dropdown startTransceiverDropdown;
    public TMP_Dropdown startAntennaDropdown;
    public TMP_Dropdown endTransceiverDropdown;
    public TMP_Dropdown endAntennaDropdown; 
    
    public TMP_InputField pathLossInput;
    public TMP_InputField shadowingEffectInput;
    public TMP_InputField multipathEffectInput;
    public TMP_InputField noisePowerDensityInput;

    public Button applyButton;                 // ������������ҹر�panel�Ľ���

    public GameObject channelPrefab;                 // ����ʵ�����Ĺܵ�ģ��

    private CombatUnit combatStart;            // �����ѡȡ��������Ԫ 
    private CombatUnit combatEnd;
    private CommChannelProperties channelProperties;

    private bool isSelectingStart = true; 


    void Start()
    {
        applyButton.onClick.AddListener(OnApplyButtonClickd); 
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���������
        {
            SelectCombatUnit();
        }
    }

    void SelectCombatUnit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            CombatUnit selectedUnit = hit.collider.GetComponent<CombatUnit>();
            if (selectedUnit != null)
            {
                if (isSelectingStart)
                {
                    combatStart = selectedUnit;
                    isSelectingStart = false; // ��һ�ε��ѡendUnit
                    Debug.Log("Selected Start Unit: " + combatStart.name);
                    InitializeDropdown(combatStart, startTransceiverDropdown, startAntennaDropdown);
                }
                else
                {
                    combatEnd = selectedUnit;
                    Debug.Log("Selected End Unit: " + combatEnd.name);
                    isSelectingStart = true; // �ٴε��ʱѡ��startUnit
                    InitializeDropdown(combatEnd, endTransceiverDropdown, endAntennaDropdown);
                }
            }
        }
    }

    void InitializeDropdown(CombatUnit unit , TMP_Dropdown transceiverDropdown , TMP_Dropdown attenaDropdown)
    {
        // ��ʼ������ģ�����ʾ
        transceiverDropdown.ClearOptions();
        List<string> transceiverOptions = new List<string>(); 
        for (int i = 0; i < unit.transceivers.Count; i++)
        {
            transceiverOptions.Add("Tranceivers" + i); 

        }
        transceiverDropdown.AddOptions(transceiverOptions);

        attenaDropdown.ClearOptions();
        List<string> antennaOptions = new List<string>();
        for (int i = 0; i < unit.antennas.Count; i++)
        {
            antennaOptions.Add("Antenna " + i);
        }
        attenaDropdown.AddOptions(antennaOptions);

    }




    void OnApplyButtonClickd()
    {
        if (combatStart !=null && combatEnd != null)
        {
            GameObject newChannel = Instantiate(channelPrefab); 

            channelProperties = newChannel.GetComponent<CommChannelProperties>();
            CommChannelController channelController = newChannel.GetComponent<CommChannelController>();

            channelProperties.startUnit = combatStart;
            channelProperties.endUnit = combatEnd;

            // ��ֵ�� CommChannelController �е� startTransform �� endTransform
            channelController.startTransform = combatStart.transform;
            channelController.endTransform = combatEnd.transform;

            // ���ݳ����е�Dropdown�������index����ֵ
            channelProperties.startTransceiverIndex = startTransceiverDropdown.value;
            channelProperties.startAntennaIndex = startAntennaDropdown.value;
            channelProperties.endTransceiverIndex = endTransceiverDropdown.value;
            channelProperties.endAntennaIndex = endAntennaDropdown.value;

            // ��ȡ�����еĶ�Ӧ��Input��������ݲ��Ҹı��Ӧ�ű��ļ���Ĳ���
            channelProperties.pathLoss = float.Parse(pathLossInput.text);
            channelProperties.shadowingEffect = float.Parse(shadowingEffectInput.text);
            channelProperties.multipathEffect = float.Parse(multipathEffectInput.text);
            channelProperties.noisePowerDensity = float.Parse(noisePowerDensityInput.text);
            
            // ��ʾ�������ж�Ӧ�Ĺܵ�ģ��
            channelProperties.UpdateChannelVisuals();

            // �رյ�ǰ���
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Start and End units must be selected."); 
        }

    }

    
}
