using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 
public class UIController : MonoBehaviour
{
    public TransceiverPanel Transceiverpanel;
    public AntennaPanel Antennapanel;

    public Button AddTransButton;        //�������Ƴ����е���Ӷ�Ӧ����İ�ť
    public Button AddAtennaButton;

    private CombatUnit combatunit;

    private Transceiver transceiver;
    private Antenna antenna;



    private void Awake()
    {
        ClickController.OnselectCombatUnitChanged += HandleSelectCombatunitChanged;
        //HandleSelectCombatunitChanged(combatunit);
    }
    private void Start()
    {
        transceiver = new Transceiver();
        antenna = new Antenna(); 
    }
    private void OnDestroy()
    {
       ClickController.OnselectCombatUnitChanged -= HandleSelectCombatunitChanged;
    }

    // Bind ��������ˢ����ʾÿһ�ε�Panel ��Clickcontrollerÿһ��ѡ�񵽶�Ӧ��CombatUnit֮������µ���Bind����
    private void HandleSelectCombatunitChanged(CombatUnit _combatUnit)
    {
        combatunit = _combatUnit;        // ��õ��������Ķ���
        if (Transceiverpanel != null)
        {
            Transceiverpanel.Bind(_combatUnit); 
        }
        if (Antennapanel != null)
        {
            Antennapanel.Bind(_combatUnit); 
        }
        // �˴���д��ť�󶨵��߼�
        if (combatunit != null)
        {
            HandleButtonClick(combatunit); 
        }
    }
    
    private void HandleButtonClick(CombatUnit _combatUnit)
    {
        AddTransButton.onClick.RemoveAllListeners();
        
        AddTransButton.onClick.AddListener(() => CreateAndAddNewTransceiver(transceiver,_combatUnit));

        AddAtennaButton.onClick.RemoveAllListeners();
        AddAtennaButton.onClick.AddListener(() => CreateAndAddNewAntenna(antenna , _combatUnit)); 
    }
    
    private void CreateAndAddNewTransceiver(Transceiver _transceiver , CombatUnit _combatUnit)
    {
        _combatUnit.AddTransceiver(_transceiver);
        transceiver = new Transceiver();       // Ϊ��һ�θ�ֵ����׼��
        Transceiverpanel.Bind(_combatUnit);    // ���±������������
    }

    private void CreateAndAddNewAntenna(Antenna _antenna , CombatUnit _combatUnit)
    {
        _combatUnit.AddAntenna(_antenna);
        antenna = new Antenna();
        Antennapanel.Bind(_combatUnit);
    }
}
