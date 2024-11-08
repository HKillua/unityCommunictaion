using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;
// 绑定到摄像机上
public class ClickController : MonoBehaviour
{
    [SerializeField] private Camera chooseCamera;
    public static event Action<CombatUnit> OnselectCombatUnitChanged;  //当选中的对象发生变化的时候会执行的操作
    public static CombatUnit SelectedCombat;


    //public UILoader sceneUILoader;    // 控制场景UI加载器


    private void Start()
    {
        chooseCamera = GameObject.FindObjectOfType<Camera>();
        //sceneUILoader = GetComponent<UILoader>();    //目的是之后更改UIloader里面的事件函数
        //sceneUILoader.OnSceneChanged += handleSceneChanged; 
    }

    private void OnDestroy()
    {
        //sceneUILoader.OnSceneChanged -= handleSceneChanged; 

    }

    private void Update()
    {
       if (Input.GetButtonDown("Fire1"))    //监听的是鼠标左键
        {
            var ray = chooseCamera.ScreenPointToRay(Input.mousePosition);
            Debug.Log("click object"); 
            if (Physics.Raycast(ray,out var hitInfo))
            {
                var combatunit = hitInfo.collider.GetComponent<CombatUnit>(); //得到的鼠标选中的模型上的CombatUnit
                SelectedCombat = combatunit;
                OnselectCombatUnitChanged?.Invoke(SelectedCombat); 
            }
        }
       if (Input.GetKeyDown(KeyCode.Escape))
        {
            SelectedCombat = null;
            OnselectCombatUnitChanged?.Invoke(SelectedCombat); 
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (SelectedCombat != null)
            {
               // SelectedCombat.Damage(); 
            }
        }

    }

    private void handleSceneChanged(CombatUnit combatunit)
    {
        OnselectCombatUnitChanged?.Invoke(combatunit);
    }

    // 用于清楚删除场景中的模型之后清除窗口的UI函数
    public static void ClearUIscene()
    {
        OnselectCombatUnitChanged?.Invoke(null); 
    }
}
