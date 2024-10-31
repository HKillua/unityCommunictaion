using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public TransceiverPanel Transceiverpanel;
    public AntennaPanel Antennapanel;

   // private CombatUnit combatunit;    // 这个东西的作用是干什么的？combatUnit应该是鼠标获取到的数值才对

    private void Awake()
    {
        ClickController.OnselectCombatUnitChanged += HandleSelectCombatunitChanged;
        //HandleSelectCombatunitChanged(combatunit);
    }

    private void OnDestroy()
    {
       ClickController.OnselectCombatUnitChanged -= HandleSelectCombatunitChanged;
    }

    // Bind 的作用是刷新显示每一次的Panel ，Clickcontroller每一次选择到对应的CombatUnit之后会重新调用Bind函数
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
