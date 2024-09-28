using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalLocalPointerPosition;
    private Vector2 originalPanelLocalPosition;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        Debug.Log("draggable panel awake");
    }
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 通过Canvas的RectTransform获取鼠标的本地坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out originalLocalPointerPosition);
        // 记录当前面板的位置
        originalPanelLocalPosition = rectTransform.localPosition;

        // 计算相对偏移量，确保在拖动过程中保持一致
        Vector2 offset = originalPanelLocalPosition - originalLocalPointerPosition;
        originalLocalPointerPosition += offset;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rectTransform == null || canvas == null)
            return;

        Vector2 localPointerPosition;
        // 通过Canvas的RectTransform获取当前鼠标的本地坐标
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out localPointerPosition))
        {
            Vector2 offset = localPointerPosition - originalLocalPointerPosition;
            rectTransform.localPosition = originalPanelLocalPosition + offset;
        }
    }

}
