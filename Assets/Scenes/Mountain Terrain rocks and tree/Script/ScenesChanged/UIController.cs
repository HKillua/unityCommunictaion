using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public TransceiverPanel Transceiverpanel;
    public AntennaPanel Antennapanel;

   // private CombatUnit combatunit;    // ��������������Ǹ�ʲô�ģ�combatUnitӦ��������ȡ������ֵ�Ŷ�

    private void Awake()
    {
        ClickController.OnselectCombatUnitChanged += HandleSelectCombatunitChanged;
        //HandleSelectCombatunitChanged(combatunit);
    }

    private void OnDestroy()
    {
       ClickController.OnselectCombatUnitChanged -= HandleSelectCombatunitChanged;
    }

    // Bind ��������ˢ����ʾÿһ�ε�Panel ��Clickcontrollerÿһ��ѡ�񵽶�Ӧ��CombatUnit֮������µ���Bind����
    private void HandleSelectCombatunitChanged(CombatUnit _combatUnit)
    {
        if (Transceiverpanel != null)
        {
            Transceiverpanel.Bind(_combatUnit); 
        }
        if (Antennapanel != null)
        {
            //Antennapanel.Bind(_combatUnit); 
        }
    }

}
