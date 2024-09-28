using System.Collections;
using UnityEngine;


public class SlidingPanel : MonoBehaviour
{
    public RectTransform panel; // �������ڵ� RectTransform
    public float slideDuration = 0.5f; // ��������ʱ��
    private Vector2 hiddenPosition;
    private Vector2 visiblePosition;
    private bool isVisible = false;

    void Start()
    {
        // ��̬������ʼ�ͽ���λ��
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
