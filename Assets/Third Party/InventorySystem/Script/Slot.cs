using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour,IDropHandler
{
    [HideInInspector]
    public bool isFill=false;
    [HideInInspector]
    public bool isFull=false;

    [HideInInspector]
    public int slotId;

    [HideInInspector] 
    public Inventory inventory;

    public void OnDrop(PointerEventData eventData)
    {
        var inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (inventoryItem != null)
        {
            UnityEngine.Debug.Log($"Drop to slot {slotId}");
            inventory.DropItem(this,inventoryItem);
        }   
        
    }

    public void DropItem(InventoryItem droppedItem)
    {
        droppedItem.transform.SetParent(transform);
        droppedItem.rectTransform.anchoredPosition=Vector2.zero;
        droppedItem.locateSlotId=slotId;
        droppedItem.inventory=inventory;
        isFill=true;
    }
}
