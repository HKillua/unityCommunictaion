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
        // ͨ��Canvas��RectTransform��ȡ���ı�������
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out originalLocalPointerPosition);
        // ��¼��ǰ����λ��
        originalPanelLocalPosition = rectTransform.localPosition;

        // �������ƫ������ȷ�����϶������б���һ��
        Vector2 offset = originalPanelLocalPosition - originalLocalPointerPosition;
        originalLocalPointerPosition += offset;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rectTransform == null || canvas == null)
            return;

        Vector2 localPointerPosition;
        // ͨ��Canvas��RectTransform��ȡ��ǰ���ı�������
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out localPointerPosition))
        {
            Vector2 offset = localPointerPosition - originalLocalPointerPosition;
            rectTransform.localPosition = originalPanelLocalPosition + offset;
        }
    }

}
