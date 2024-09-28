using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonforPanel : MonoBehaviour
{
    public GameObject uiPanelprefab;
    public Vector2 panelOffsetMin = new Vector2(0, 185); // 左下角的偏移值 (left, bottom)
    public Vector2 panelOffsetMax = new Vector2(742, 0); // 右上角的偏移值 (right, top)
    public Vector2 panelsizeDelta = new Vector2(200, 200); 



    private GameObject uiPanel; // 参数配置UI界面
    void Start()
    {

        if (uiPanelprefab != null)
        {

            uiPanel = Instantiate(uiPanelprefab);
            uiPanel.transform.SetParent(transform.root);
            uiPanel.SetActive(false); // 初始化时隐藏UI界面
            Debug.Log("button for panel instantiate");

        }

        // 获取按钮组件，并注册点击事件
        Button openButton = GetComponent<Button>();
        if (openButton != null)
        {
            openButton.onClick.AddListener(Open_Window);
        }
        else
        {
            Debug.LogError("CloseButton component is not found on the game object.");
        }
    }


    void Open_Window()
    {
        if (uiPanel != null)
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

                /*RectTransform canvasRect = transform.root.GetComponent<RectTransform>();
                Vector2 anchoredPosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, Camera.main, out anchoredPosition);

                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

                rectTransform.pivot = new Vector2(0.5f, 0.5f);

                rectTransform.anchoredPosition = anchoredPosition;   // 这部分代码保证了中心点和锚点的设置正确

                rectTransform.sizeDelta = new Vector2(200,200);   // 设置你要渲染出来的界面的宽度和高度 可以先去试图方面预览一下，然后选择合适的
*/

                /*rectTransform.offsetMin = panelOffsetMin;
                rectTransform.offsetMax = panelOffsetMax;*/
            }
            uiPanel.SetActive(true);
        }
    }



}
