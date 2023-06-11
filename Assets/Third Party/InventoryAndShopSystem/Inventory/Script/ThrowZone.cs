using UnityEngine;
using UnityEngine.EventSystems;
public class ThrowZone : MonoBehaviour,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag.GetComponent<InventoryItem>() != null)
        {
            Debug.Log("Throw item");
            Inventory.instance.SetNullDraggedItem();
            Destroy(eventData.pointerDrag.gameObject);
        }    
    }
}
