using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonforPanel : MonoBehaviour
{
    public GameObject uiPanelprefab;
    public Vector2 panelOffsetMin = new Vector2(0, 185); // ���½ǵ�ƫ��ֵ (left, bottom)
    public Vector2 panelOffsetMax = new Vector2(742, 0); // ���Ͻǵ�ƫ��ֵ (right, top)
    public Vector2 panelsizeDelta = new Vector2(200, 200); 



    private GameObject uiPanel; // ��������UI����
    void Start()
    {

        if (uiPanelprefab != null)
        {

            uiPanel = Instantiate(uiPanelprefab);
            uiPanel.transform.SetParent(transform.root);
            uiPanel.SetActive(false); // ��ʼ��ʱ����UI����
            Debug.Log("button for panel instantiate");

        }

        // ��ȡ��ť�������ע�����¼�
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

                // ����ê��Ͷ��뷽ʽ
                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                rectTransform.pivot = new Vector2(0.5f, 0.5f);

                // �����λ������Ϊ����
                rectTransform.anchoredPosition = Vector2.zero;
                rectTransform.sizeDelta = panelsizeDelta;

                /*RectTransform canvasRect = transform.root.GetComponent<RectTransform>();
                Vector2 anchoredPosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, Camera.main, out anchoredPosition);

                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

                rectTransform.pivot = new Vector2(0.5f, 0.5f);

                rectTransform.anchoredPosition = anchoredPosition;   // �ⲿ�ִ��뱣֤�����ĵ��ê���������ȷ

                rectTransform.sizeDelta = new Vector2(200,200);   // ������Ҫ��Ⱦ�����Ľ���Ŀ�Ⱥ͸߶� ������ȥ��ͼ����Ԥ��һ�£�Ȼ��ѡ����ʵ�
*/

                /*rectTransform.offsetMin = panelOffsetMin;
                rectTransform.offsetMax = panelOffsetMax;*/
            }
            uiPanel.SetActive(true);
        }
    }



}
