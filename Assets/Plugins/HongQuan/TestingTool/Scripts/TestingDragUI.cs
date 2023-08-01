using UnityEngine;
using UnityEngine.EventSystems;

public class TestingDragUI : MonoBehaviour, IDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;

    private void Start()
    {
        canvas = TestingMenu.instance.canvas;
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
}
