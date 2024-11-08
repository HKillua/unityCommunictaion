using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 
public class UIController : MonoBehaviour
{
    public TransceiverPanel Transceiverpanel;
    public AntennaPanel Antennapanel;

    public Button AddTransButton;        //用来控制场景中的添加对应组件的按钮
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

    // Bind 的作用是刷新显示每一次的Panel ，Clickcontroller每一次选择到对应的CombatUnit之后会重新调用Bind函数
    private void HandleSelectCombatunitChanged(CombatUnit _combatUnit)
    {
        combatunit = _combatUnit;        // 获得到鼠标点击后的对象
        if (Transceiverpanel != null)
        {
            Transceiverpanel.Bind(_combatUnit); 
        }
        if (Antennapanel != null)
        {
            Antennapanel.Bind(_combatUnit); 
        }
        // 此处编写按钮绑定的逻辑
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
        transceiver = new Transceiver();       // 为下一次赋值做好准备
        Transceiverpanel.Bind(_combatUnit);    // 更新变量里面的数据
    }

    private void CreateAndAddNewAntenna(Antenna _antenna , CombatUnit _combatUnit)
    {
        _combatUnit.AddAntenna(_antenna);
        antenna = new Antenna();
        Antennapanel.Bind(_combatUnit);
    }
}
