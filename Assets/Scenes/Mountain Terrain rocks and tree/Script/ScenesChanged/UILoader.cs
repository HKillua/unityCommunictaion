using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILoader : MonoBehaviour
{
    public event Action<CombatUnit> OnSceneChanged; // �����仯��ʱ��������ʾ��UI
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (SceneManager.GetSceneByName("UIScene").isLoaded == false)
                SceneManager.LoadSceneAsync("UIScene", LoadSceneMode.Additive);
            else
            {
                SceneManager.UnloadSceneAsync("UIScene"); 
            }
        }
    }

}
