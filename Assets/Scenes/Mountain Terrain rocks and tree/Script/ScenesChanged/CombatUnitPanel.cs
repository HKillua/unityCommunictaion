using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class CombatUnitPanel : MonoBehaviour
{
    public GameObject itemUIPrefab;  // ������ʾ��UIģ��
    public Transform panelContainer; // ���ڷ���UI��content

    public static event Action UpdateCombatUI; 

    private List<CombatUnit> combatUnits = new List<CombatUnit>();

    private void Awake()
    {
        UpdateCombatUI += UpdateCombatUnitList; 
    }


    private void OnDestroy()
    {
        UpdateCombatUI -= UpdateCombatUnitList; 
    }

    public static void UpdateUITrigger()
    {
        UpdateCombatUI?.Invoke(); 
    }

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
            item.GetComponentInChildren<Image>().sprite = unit.CombatImage;
            Button removeButton = item.GetComponentInChildren<Button>();
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

        ClickController.ClearUIscene();     //ɾ�������е�����֮��Ҫȥ���
        UpdateCombatUnitList();
    }

}
