using UnityEngine;
using UnityEngine.EventSystems;

public class DoubleClickHandler : MonoBehaviour
{
    public GameObject  uiPanelprefab;
    //public Vector2 panelOffsetMin = new Vector2(0, 185); // 左下角的偏移值 (left, bottom)
    //public Vector2 panelOffsetMax = new Vector2(742, 0); // 右上角的偏移值 (right, top)

    public Vector2 panelsizeDelta = new Vector2(1000, 800); 


    private GameObject uiPanel; // 参数配置UI界面
    private float lastClickTime;
    private const float doubleClickThreshold = 0.3f; // 双击时间间隔

    void Start()
    {   

        if (uiPanelprefab != null)
        {
            Canvas canvas = FindObjectOfType<Canvas>(); 
            if (canvas != null)
            {
                uiPanel = Instantiate(uiPanelprefab);
                uiPanel.transform.SetParent(canvas.transform,false);
                uiPanel.SetActive(false);
                Debug.Log("double click instantiate");

            }
            else
            {
                Debug.LogError("Canvas not found in the scene."); 
            }

        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float timeSinceLastClick = Time.time - lastClickTime;

            if (timeSinceLastClick <= doubleClickThreshold)
            {
                CheckDoubleClick();
            }
            lastClickTime = Time.time;
        }

    }

    void CheckDoubleClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))   // 检测到点击对应的物体
        {
            

            if (hit.transform == transform)
            {
                
                ToggleUIPanel(true);
            }
        }
        
    }

    /*void ToggleUIPanel(bool enable)
    {
        if (uiPanel != null)
        {

            if (enable)
            {
                RectTransform rectTransform = uiPanel.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    RectTransform canvasRect = transform.root.GetComponent<RectTransform>();
                    Vector2 anchoredPosition;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, Camera.main, out anchoredPosition);

                    rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

                    rectTransform.pivot = new Vector2(0.5f, 0.5f);

                    rectTransform.anchoredPosition = anchoredPosition;

                    rectTransform.sizeDelta = panelsizeDelta;


                    *//*rectTransform.offsetMin = panelOffsetMin;
                    rectTransform.offsetMax = panelOffsetMax;*//*
                }
               
            }

            uiPanel.SetActive(enable);
        }
        
    }*/

    void ToggleUIPanel(bool enable)
    {
        if (uiPanel != null)
        {
            if (enable)
            {
                RectTransform rectTransform = uiPanel.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    RectTransform canvasRect = transform.root.GetComponent<RectTransform>();

                    // 设置锚点和对齐方式
                    rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                    rectTransform.pivot = new Vector2(0.5f, 0.5f);

                    // 将面板位置设置为中心
                    rectTransform.anchoredPosition = Vector2.zero;
                    rectTransform.sizeDelta = panelsizeDelta;
                }
            }

            uiPanel.SetActive(enable);
        }
    }
}
