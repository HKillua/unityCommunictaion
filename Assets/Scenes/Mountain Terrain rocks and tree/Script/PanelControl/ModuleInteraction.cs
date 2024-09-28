using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModuleInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    private ToolbarManager toolbarManager; 
    void Start()
    {
        toolbarManager = FindObjectOfType<ToolbarManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject selectedObject = hit.collider.gameObject;
                HandleInteraction(selectedObject);
            }
        }
    }

    void HandleInteraction(GameObject seletedObject)
    {
        switch (toolbarManager.currentTool)
        {
            case ToolbarManager.ToolType.Create:
                break;
            case ToolbarManager.ToolType.Show:
                break;
            case ToolbarManager.ToolType.Run:
                break;
             
        }
    }
}
