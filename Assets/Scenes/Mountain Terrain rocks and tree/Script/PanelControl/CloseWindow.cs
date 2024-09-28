using UnityEngine;
using UnityEngine.UI;
public class CloseWindow : MonoBehaviour
{
    private GameObject windowToClose; // ��Ҫ�رյĴ��ڶ���

    void Start()
    {
        // ��ȡ��������Ϊ��Ҫ�رյĴ��ڶ���
        windowToClose = transform.parent.gameObject;

        // ��ȡ��ť�������ע�����¼�
        Button closeButton = GetComponent<Button>();
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(Close_Window);
        }
        else
        {
            Debug.LogError("CloseButton component is not found on the game object.");
        }
    }

    void Close_Window()
    {
        // �رմ���
        if (windowToClose != null)
        {
            windowToClose.SetActive(false);
        }
    }
}
