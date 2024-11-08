using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;
// �󶨵��������
public class ClickController : MonoBehaviour
{
    [SerializeField] private Camera chooseCamera;
    public static event Action<CombatUnit> OnselectCombatUnitChanged;  //��ѡ�еĶ������仯��ʱ���ִ�еĲ���
    public static CombatUnit SelectedCombat;


    //public UILoader sceneUILoader;    // ���Ƴ���UI������


    private void Start()
    {
        chooseCamera = GameObject.FindObjectOfType<Camera>();
        //sceneUILoader = GetComponent<UILoader>();    //Ŀ����֮�����UIloader������¼�����
        //sceneUILoader.OnSceneChanged += handleSceneChanged; 
    }

    private void OnDestroy()
    {
        //sceneUILoader.OnSceneChanged -= handleSceneChanged; 

    }

    private void Update()
    {
       if (Input.GetButtonDown("Fire1"))    //��������������
        {
            var ray = chooseCamera.ScreenPointToRay(Input.mousePosition);
            Debug.Log("click object"); 
            if (Physics.Raycast(ray,out var hitInfo))
            {
                var combatunit = hitInfo.collider.GetComponent<CombatUnit>(); //�õ������ѡ�е�ģ���ϵ�CombatUnit
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

    // �������ɾ�������е�ģ��֮��������ڵ�UI����
    public static void ClearUIscene()
    {
        OnselectCombatUnitChanged?.Invoke(null); 
    }
}
