using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    [Header("UI")]
    public RectTransform rectTransform;
    public Text amoutText;
    public Image image;
    private CanvasGroup canvasGroup;
    [HideInInspector] public Canvas canvas;
    [HideInInspector] public int locateSlotId;
    [HideInInspector] public Inventory inventory;
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
        int draggedItemAmount=inventory.InventoryData.Data[locateSlotId].amount;
        if ((inventory.takeHalfItemButton.isHolding||Input.GetKey(KeyCode.LeftShift)) && draggedItemAmount!=1)
        {
            Debug.Log("Drag half item");
            transform.SetParent(canvas.transform);
            transform.SetAsLastSibling();
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.8f;

            int draggedAmount = 0;
            int amount = inventory.InventoryData.Data[locateSlotId].amount;

            draggedAmount = amount / 2 + (amount % 2 == 0 ? 0 : 1);

            if (draggedAmount > 1) amoutText.text = draggedAmount.ToString(); else amoutText.text = "";

            inventory.SetDraggedItem(this, draggedAmount);
        }
        else if ((inventory.takeOneItemButton.isHolding||Input.GetMouseButton(1))&& draggedItemAmount!=1)
        {
            Debug.Log("Drag 1 item");
            transform.SetParent(canvas.transform);
            transform.SetAsLastSibling();
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.8f;
            amoutText.text = "";

            inventory.SetDraggedItem(this, 1);
        }
        else
        {
            Debug.Log("Drag all item");
            transform.SetParent(canvas.transform);
            transform.SetAsLastSibling();
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.8f;

            inventory.SetDraggedItem(this);
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
            inventory.CombackToLastSlot(this);
    }


    #endregion Drag and Drop

    public void SetData(Canvas canvas, Item item,Inventory inventory)
    {
        transform.SetParent(inventory.slots[item.slotId].transform,false);
        rectTransform.anchoredPosition = Vector3.zero;
        rectTransform.localScale = Vector3.one;
        this.canvas = canvas;
        locateSlotId = item.slotId;
        this.inventory = inventory;

        if (item.amount > 1) amoutText.text = item.amount.ToString();
        image.sprite = item.icon;   
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        inventory.ShowInfo(locateSlotId);
        inventory.SelectItem(locateSlotId);
    }
}
