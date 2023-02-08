using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [Header("UI")]
    public RectTransform rectTransform;
    public Text amoutText;
    public Image image;
    private CanvasGroup canvasGroup;
    [HideInInspector] public Canvas canvas;
    [HideInInspector] public int locateSlotId;
    //[HideInInspector] public bool canDrag=false;

    private void Awake()
    {
        InitUI();
    }

    #region Drag and Drop
    private void InitUI()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        int draggedItemAmount=Inventory.instance.InventoryData.Data[locateSlotId].amount;
        if ((Inventory.instance.takeHalfItemButton.isHolding||Input.GetKey(KeyCode.LeftShift)) && draggedItemAmount!=1)
        {
            Debug.Log("Drag half item");
            transform.SetParent(canvas.transform);
            transform.SetAsLastSibling();
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.8f;

            int draggedAmount = 0;
            int amount = Inventory.instance.InventoryData.Data[locateSlotId].amount;

            draggedAmount = amount / 2 + (amount % 2 == 0 ? 0 : 1);

            if (draggedAmount > 1) amoutText.text = draggedAmount.ToString(); else amoutText.text = "";

            Inventory.instance.SetDraggedItem(this, draggedAmount);
        }
        else if ((Inventory.instance.takeOneItemButton.isHolding||Input.GetMouseButton(1))&& draggedItemAmount!=1)
        {
            Debug.Log("Drag 1 item");
            transform.SetParent(canvas.transform);
            transform.SetAsLastSibling();
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.8f;
            amoutText.text = "";

            Inventory.instance.SetDraggedItem(this, 1);
        }
        else
        {
            Debug.Log("Drag all item");
            transform.SetParent(canvas.transform);
            transform.SetAsLastSibling();
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.8f;

            Inventory.instance.SetDraggedItem(this);
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        if (transform.parent == canvas.transform)
            Inventory.instance.CombackToLastSlot();
    }


    #endregion Drag and Drop

    public void SetData(Canvas canvas, Item item)
    {
        transform.SetParent(Inventory.instance.slots[item.slotId].transform);
        rectTransform.anchoredPosition = Vector3.zero;
        rectTransform.localScale = Vector3.one;
        this.canvas = canvas;
        locateSlotId = item.slotId;

        if (item.amount > 1) amoutText.text = item.amount.ToString();
        image.sprite = Resources.Load<Sprite>($"Image/{item.itemName}");
    }
}
