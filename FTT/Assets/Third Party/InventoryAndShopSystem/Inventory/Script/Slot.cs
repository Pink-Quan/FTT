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

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag.GetComponent<InventoryItem>() != null)
        {
            UnityEngine.Debug.Log($"Drop to slot {slotId}");
            Inventory.instance.DropItem(this);
        }   
        
    }

    public void DropItem(InventoryItem droppedItem)
    {
        droppedItem.transform.SetParent(transform);
        droppedItem.rectTransform.anchoredPosition=Vector2.zero;
        droppedItem.locateSlotId=slotId;
        isFill=true;
    }
}
