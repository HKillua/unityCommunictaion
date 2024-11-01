using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnitPanel : MonoBehaviour
{
    public GameObject itemUIPrefab;  // 用于显示的UI模板
    public Transform panelContainer; // 用于放置UI的content

    private List<CombatUnit> combatUnits = new List<CombatUnit>(); 

    void Start()
    {
        UpdateCombatUnitList(); 
    }

    public void UpdateCombatUnitList()
    { 
        foreach(Transform child in panelContainer)
        {
            Destroy(child.gameObject);
        }

        combatUnits = new List<CombatUnit>(FindObjectsOfType<CombatUnit>());

        foreach (CombatUnit unit in combatUnits)
        {
            GameObject item = Instantiate(itemUIPrefab,panelContainer);
            // 此处可以添加依据CombatUnit里面的信息进行UI部分的显示

            
        }
           
    }

    public void RemoveCombatUnit(CombatUnit unit)
    {
        Destroy (unit.gameObject);

        UpdateCombatUnitList(); 
    }

    

}
