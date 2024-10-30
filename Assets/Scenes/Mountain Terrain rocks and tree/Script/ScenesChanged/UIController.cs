using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public TransceiverPanel Transceiverpanel;
    public AntennaPanel Antennapanel;

    private CombatUnit combatunit;

    private void Awake()
    {
        ClickController.OnselectCombatUnitChanged += HandleSelectCombatunitChanged;
        HandleSelectCombatunitChanged(combatunit);
    }

    private void OnDestroy()
    {
       ClickController.OnselectCombatUnitChanged -= HandleSelectCombatunitChanged;
    }

    private void HandleSelectCombatunitChanged(CombatUnit _combatUnit)
    {
        if (Transceiverpanel != null)
        {
            //Transceiverpanel.Bind(_combatUnit); 
        }
        if (Antennapanel != null)
        {
            //Antennapanel.Bind(_combatUnit); 
        }
    }

}
