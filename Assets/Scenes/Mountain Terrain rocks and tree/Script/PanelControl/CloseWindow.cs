using UnityEngine;
using UnityEngine.UI;
public class CloseWindow : MonoBehaviour
{
    private GameObject windowToClose; // 需要关闭的窗口对象

    void Start()
    {
        // 获取父对象作为需要关闭的窗口对象
        windowToClose = transform.parent.gameObject;

        // 获取按钮组件，并注册点击事件
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
        // 关闭窗口
        if (windowToClose != null)
        {
            windowToClose.SetActive(false);
        }
    }
}
