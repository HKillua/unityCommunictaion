using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnitPanel : MonoBehaviour
{
    public GameObject itemUIPrefab;  // ������ʾ��UIģ��
    public Transform panelContainer; // ���ڷ���UI��content

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
            // �˴������������CombatUnit�������Ϣ����UI���ֵ���ʾ

            
        }
           
    }

    public void RemoveCombatUnit(CombatUnit unit)
    {
        Destroy (unit.gameObject);

        UpdateCombatUnitList(); 
    }

    

}
