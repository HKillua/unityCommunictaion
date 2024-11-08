using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class CombatUnitPanel : MonoBehaviour
{
    public GameObject itemUIPrefab;  // 用于显示的UI模板
    public Transform panelContainer; // 用于放置UI的content

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

            // 此处可以添加依据CombatUnit里面的信息进行UI部分的显示、

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

        ClickController.ClearUIscene();     //删除场景中的物体之后还要去完成
        UpdateCombatUnitList();
    }

}
