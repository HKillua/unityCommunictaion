using System.Collections;
using UnityEngine;


public class SlidingPanel : MonoBehaviour
{
    public RectTransform panel; // 滑动窗口的 RectTransform
    public float slideDuration = 0.5f; // 动画持续时间
    private Vector2 hiddenPosition;
    private Vector2 visiblePosition;
    private bool isVisible = false;

    void Start()
    {
        // 动态计算起始和结束位置
        hiddenPosition = new Vector2(0, panel.anchoredPosition.y);
        visiblePosition = new Vector2(-panel.rect.width, panel.anchoredPosition.y);

    }

    public void TogglePanel()
    {
        if (isVisible)
        {
            StartCoroutine(SlidePanel(hiddenPosition));
        }
        else
        {
            StartCoroutine(SlidePanel(visiblePosition));
        }
        isVisible = !isVisible;
    }

    private IEnumerator SlidePanel(Vector2 targetPosition)
    {
        Vector2 startPosition = panel.anchoredPosition;
        float elapsedTime = 0;

        while (elapsedTime < slideDuration)
        {
            panel.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / slideDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        panel.anchoredPosition = targetPosition;
    }
}
