using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
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
            // �˴������������CombatUnit�������Ϣ����UI���ֵ���ʾ��
            item.GetComponentInChildren<TMP_Text>().text = unit.gameObject.name;

            Button removeButton = item.GetComponentInChildren<Button>();
            //if (removeButton == null) Debug.Log("this is a null button"); 
            removeButton.onClick.AddListener(() => RemoveCombatUnit(unit)); 
        }
           
    }

    public void RemoveCombatUnit(CombatUnit unit)
    {
        Destroy(unit.gameObject);

        // UpdateCombatUnitList();  
        StartCoroutine(DelaydUpdateCombatUnitList()); 
    }

    private IEnumerator DelaydUpdateCombatUnitList()
    {
        yield return null;

        UpdateCombatUnitList();
    }

}
