using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class ThrowZone : MonoBehaviour,IDropHandler
{
    [HideInInspector] public Inventory inventory;
    public void OnDrop(PointerEventData eventData)
    {
        var item = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (item != null)
        {
            Debug.Log("Throw item");
            item.inventory.ThrowItem();
            Destroy(item.gameObject);
        }    
    }
}

public class ThrowItemEvent : UnityEvent<Item> { }
