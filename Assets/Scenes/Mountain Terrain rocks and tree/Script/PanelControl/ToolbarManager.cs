using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 



public class ToolbarManager : MonoBehaviour
{
    // Start is called before the first frame update
    public enum ToolType {None,Create,Show,Run}
    public ToolType currentTool = ToolType.Create;

    public Button CreatButton;
    public Button ShowButton; 
    public Button RunButton;

    

    void Start()
    {
        CreatButton.onClick.AddListener(() => SelectTool(ToolType.Create));
        ShowButton.onClick.AddListener(() => SelectTool(ToolType.Show));
        RunButton.onClick.AddListener(() => SelectTool(ToolType.Run)); 
    }


    void SelectTool(ToolType tool)
    {
        currentTool = tool; 

    }
    // Update is called once per frame
    
}
