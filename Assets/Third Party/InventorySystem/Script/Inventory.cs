using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Canvas inventoryCanvas;
    [SerializeField] private ThrowZone throwZone;
    [SerializeField] private InventoryItemInfo inforBoard;
    public List<Slot> slots;
    [SerializeField] private InventoryItem inventoryPrefab;

    [HideInInspector] public InventoryData InventoryData;
    [HideInInspector] public InventoryItem[] InventoryItemsArray;

    [HideInInspector] public InventoryItem draggedItem;
    [HideInInspector] public Item draggedItemData;
    [HideInInspector] public int lastSlotID;

    public UIButton takeOneItemButton;
    public UIButton takeHalfItemButton;
    public RectTransform board;
    public GameObject buttons;

    public ThrowItemEvent OnThrowItem;
    [Serializable]
    public class SelectItemEvent : UnityEvent<Item> { }
    public SelectItemEvent OnSelectItem; 
    private void Awake()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].slotId = i;
            slots[i].inventory = this;
        }
        throwZone.inventory = this;

        InventoryItemsArray = new InventoryItem[slots.Count];
        InventoryData = new InventoryData(slots.Count);

    }

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "Magnet", 1, this);
        }

    }
    /// <summary>
    /// Add a item to a slot, if success, return true, else return false 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool AddItemToInventory(Item item)
    {
        if (item.slotId != -1)
        {
            if (!slots[item.slotId].isFill)
            {
                slots[item.slotId].isFill = true;
                if (item.amount >= item.maxAmount) slots[item.slotId].isFull = true;

                if (item.amount > item.maxAmount)
                {
                    Item offsetItem = new Item(item.itemType, item.itemData, item.itemName, -1, item.maxAmount - item.amount, item.maxAmount);
                    item.amount = item.maxAmount;
                    AddItemToInventory(offsetItem);
                }

                InventoryItem tempInventoryItem = Instantiate(inventoryPrefab);
                tempInventoryItem.SetData(inventoryCanvas, item, this);

                InventoryItemsArray[item.slotId] = tempInventoryItem;
                InventoryData.Data[item.slotId] = item;
                return true;
            }
            else if (item.itemName == InventoryData.Data[item.slotId].itemName && item.itemType == InventoryData.Data[item.slotId].itemType)
            {
                if (item.amount + InventoryData.Data[item.slotId].amount <= item.maxAmount)
                {
                    InventoryData.Data[item.slotId].amount += item.amount;
                    InventoryItemsArray[item.slotId].amoutText.text = InventoryData.Data[item.slotId].amount.ToString();

                    if (InventoryData.Data[item.slotId].amount == InventoryData.Data[item.slotId].maxAmount) slots[item.slotId].isFull = true;
                }
                else
                {
                    item.amount -= InventoryData.Data[item.slotId].maxAmount - InventoryData.Data[item.slotId].amount;
                    InventoryData.Data[item.slotId].amount = InventoryData.Data[item.slotId].maxAmount;
                    InventoryItemsArray[item.slotId].amoutText.text = InventoryData.Data[item.slotId].amount.ToString();
                    slots[item.slotId].isFull = true;

                    item.slotId = -1;
                    AddItemToInventory(item);
                }
            }
        }
        else
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (!slots[i].isFill)
                {
                    item.slotId = i;
                    slots[i].isFill = true;
                    if (item.amount >= item.maxAmount) slots[item.slotId].isFull = true;

                    if (item.amount > item.maxAmount)
                    {
                        Item offsetItem = new Item(item.itemType, item.itemData, item.itemName, -1, item.amount - item.maxAmount, item.maxAmount);
                        AddItemToInventory(offsetItem);
                        item.amount = item.maxAmount;
                    }
                    InventoryItem tempInventoryItem = Instantiate(inventoryPrefab);
                    tempInventoryItem.SetData(inventoryCanvas, item, this);

                    InventoryItemsArray[i] = tempInventoryItem;
                    InventoryData.Data[i] = item;
                    return true;


                }
                else if (!slots[i].isFull)
                {
                    if (item.itemName == InventoryData.Data[i].itemName)
                        if (item.amount + InventoryData.Data[i].amount <= item.maxAmount)
                        {
                            InventoryData.Data[i].amount += item.amount;
                            InventoryItemsArray[i].amoutText.text = InventoryData.Data[i].amount.ToString();

                            if (InventoryData.Data[i].amount == InventoryData.Data[i].maxAmount) slots[i].isFull = true;
                            return true;
                        }
                        else
                        {
                            item.amount -= InventoryData.Data[i].maxAmount - InventoryData.Data[i].amount;
                            InventoryData.Data[i].amount = InventoryData.Data[i].maxAmount;
                            InventoryItemsArray[i].amoutText.text = InventoryData.Data[i].amount.ToString();
                            slots[i].isFull = true;

                            AddItemToInventory(item);
                            return true;
                        }
                }
            }
        }
        return false;
    }
    public void ClearInventory()
    {
        for (int i = 0; i < InventoryItemsArray.Length; i++)
        {
            if (InventoryItemsArray[i] != null)
            {
                Destroy(InventoryItemsArray[i].gameObject);
                InventoryItemsArray[i] = null;
            }
        }

        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].isFull = false;
            slots[i].isFill = false;
        }

        SetNullDraggedItem();

        InventoryData = new InventoryData(slots.Count);
    }
    public void CleanInventory()
    {
        Item[] cleanArray = new Item[InventoryData.Data.Length];
        Item[] sortedArray = new Item[InventoryData.Data.Length];
        int index = 0;

        for (int i = 0; i < InventoryData.Data.Length; i++)
        {
            if (InventoryData.Data[i] == null || InventoryData.Data[i].itemName == "")
                continue;
            if (InventoryData.Data[i].amount == InventoryData.Data[i].maxAmount)
            {
                cleanArray[index++] = InventoryData.Data[i];
                continue;
            }
            for (int j = i + 1; j < InventoryData.Data.Length; j++)
            {
                if (InventoryData.Data[j] == null || InventoryData.Data[j].amount == InventoryData.Data[j].maxAmount)
                    continue;
                if (string.Compare(InventoryData.Data[i].itemName, InventoryData.Data[j].itemName) == 0)
                {
                    if (InventoryData.Data[i].amount + InventoryData.Data[j].amount <= InventoryData.Data[i].maxAmount)
                    {
                        InventoryData.Data[i].amount += InventoryData.Data[j].amount;
                        InventoryData.Data[j] = null;
                        if (InventoryData.Data[i].amount == InventoryData.Data[i].maxAmount)
                            break;
                    }
                    else if (InventoryData.Data[i].amount + InventoryData.Data[j].amount > InventoryData.Data[i].maxAmount)
                    {
                        InventoryData.Data[j].amount -= InventoryData.Data[i].maxAmount - InventoryData.Data[i].amount;
                        InventoryData.Data[i].amount = InventoryData.Data[i].maxAmount;

                        break;
                    }
                }
            }
            cleanArray[index++] = InventoryData.Data[i];
        }

        List<String> nameList = new List<String>();
        for (int i = 0; i < cleanArray.Length; i++)
        {
            if (cleanArray[i] == null || cleanArray[i].itemName == "")
                continue;
            string name = cleanArray[i].itemName;
            if (!nameList.Contains(name))
                nameList.Add(name);
        }

        index = 0;
        for (int i = 0; i < nameList.Count; i++)
        {
            if (nameList[i] == "")
                continue;
            for (int j = 0; j < cleanArray.Length; j++)
            {
                if (cleanArray[j] == null)
                    continue;
                if (nameList[i] == cleanArray[j].itemName)
                {
                    sortedArray[index++] = cleanArray[j];
                }
            }
        }

        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].isFull = false;
            slots[i].isFill = false;
        }
        for (int i = 0; i < InventoryItemsArray.Length; i++)
        {
            if (InventoryItemsArray[i] != null)
            {
                Destroy(InventoryItemsArray[i].gameObject);
                InventoryItemsArray[i] = null;
            }
        }

        for (int i = 0; i < InventoryData.Data.Length; i++)
            InventoryData.Data[i] = null;

        for (int i = 0; i < sortedArray.Length; i++)
            if (sortedArray[i] == null || sortedArray[i].itemName == "")
                continue;
            else
            {
                sortedArray[i].slotId = -1;
                AddItemToInventory(sortedArray[i]);
            }

    }

    public void SetDraggedItem(InventoryItem inventoryItem, int draggedAmount)
    {
        draggedItem = inventoryItem;
        lastSlotID = draggedItem.locateSlotId;

        ItemType type = InventoryData.Data[lastSlotID].itemType;
        string name = InventoryData.Data[lastSlotID].itemName;
        draggedItemData = new Item(type, name);
        InventoryManager.instance.itemConfig.GetItemConfig(ref draggedItemData);
        draggedItemData.amount = draggedAmount;

        InventoryData.Data[lastSlotID].amount -= draggedAmount;
        InventoryItem tempInventoryItem = Instantiate(inventoryPrefab);
        tempInventoryItem.SetData(inventoryCanvas, InventoryData.Data[lastSlotID], this);
        InventoryItemsArray[lastSlotID] = tempInventoryItem;

        slots[lastSlotID].isFull = false;

        if (InventoryData.Data[lastSlotID].amount == 1) InventoryItemsArray[lastSlotID].amoutText.text = ""; else InventoryItemsArray[lastSlotID].amoutText.text = InventoryData.Data[lastSlotID].amount.ToString();

        draggedItem.locateSlotId = -1;
        draggedItemData.slotId = -1;
    }
    public void SetDraggedItem(InventoryItem inventoryItem)
    {
        draggedItem = inventoryItem;
        draggedItemData = InventoryData.Data[draggedItem.locateSlotId];
        lastSlotID = draggedItem.locateSlotId;

        InventoryData.Data[lastSlotID] = null;
        InventoryItemsArray[lastSlotID] = null;

        slots[lastSlotID].isFill = false;
        slots[lastSlotID].isFull = false;

        draggedItem.locateSlotId = -1;
        draggedItemData.slotId = -1;
    }
    public void SetDraggedItemOutsideInventory(InventoryItem draggedItem, Item itemData) //This fuction called if you want drag a item outside the inventory
    {
        this.draggedItem = draggedItem;
        draggedItemData = itemData;
        lastSlotID = -1;
    }
    public void DropItem(Slot slot, InventoryItem inventoryItem)
    {
        var draggedItemData = inventoryItem.inventory.draggedItemData;
        var draggedItem = inventoryItem.inventory.draggedItem;
        var lastSlotID = inventoryItem.inventory.lastSlotID;

        if (!slot.isFill)
        {
            Debug.Log("Drop to unfilled slot");
            slot.DropItem(inventoryItem);
            if (draggedItemData.amount == draggedItemData.maxAmount) slot.isFull = true;
            slot.isFill = true;

            InventoryData.Data[slot.slotId] = draggedItemData;
            InventoryData.Data[slot.slotId].slotId = slot.slotId;
            InventoryItemsArray[slot.slotId] = draggedItem;

        }
        else if (!slot.isFull && draggedItemData.itemName == InventoryData.Data[slot.slotId].itemName)
        {
            if (InventoryData.Data[slot.slotId].amount != InventoryData.Data[slot.slotId].maxAmount)
            {
                if (draggedItemData.amount + InventoryData.Data[slot.slotId].amount <= InventoryData.Data[slot.slotId].maxAmount)
                {
                    //Debug.Log("Drop to unfull slot and sum of item amout is less or equal than max amount");
                    InventoryData.Data[slot.slotId].amount += draggedItemData.amount;
                    InventoryItemsArray[slot.slotId].amoutText.text = InventoryData.Data[slot.slotId].amount.ToString();

                    Destroy(draggedItem.gameObject);
                    if (InventoryData.Data[slot.slotId].amount == InventoryData.Data[slot.slotId].maxAmount) slot.isFull = true;

                }
                else
                {
                    //Drop and make item in dropped slot full, there will occur redundant item data
                    draggedItemData.amount -= InventoryData.Data[slot.slotId].maxAmount - InventoryData.Data[slot.slotId].amount; //Redundant item data 

                    //Make that slot full
                    InventoryData.Data[slot.slotId].amount = InventoryData.Data[slot.slotId].maxAmount;
                    slot.isFull = true;
                    InventoryItemsArray[slot.slotId].amoutText.text = InventoryData.Data[slot.slotId].amount.ToString();

                    //Handling redundant item data
                    if (lastSlotID == -1)
                    {
                        //If that item come from no where, add item to inventory
                        AddItemToInventory(draggedItemData);
                    }
                    else if (!slots[lastSlotID].isFill)
                    {
                        //Debug.Log("Drop to unfull slot and sum of item amount is greater max amount and last slot is EMPTY");
                        inventoryItem.inventory.slots[lastSlotID].DropItem(draggedItem);
                        draggedItem.amoutText.text = draggedItemData.amount.ToString();
                        inventoryItem.inventory.InventoryItemsArray[lastSlotID] = draggedItem;
                        inventoryItem.inventory.InventoryData.Data[lastSlotID] = draggedItemData;
                        inventoryItem.inventory.InventoryData.Data[lastSlotID].slotId = lastSlotID;
                        if (inventoryItem.inventory.InventoryData.Data[lastSlotID].amount <= 1) inventoryItem.inventory.InventoryItemsArray[lastSlotID].amoutText.text = "";
                    }
                    else if (!slots[lastSlotID].isFull)
                    {
                        //Debug.Log("Drop to unfull slot and sum of item amount is greater max amount and last slot is FILLED and UNFULL");
                        inventoryItem.inventory.InventoryData.Data[lastSlotID].amount += draggedItemData.amount;
                        inventoryItem.inventory.InventoryItemsArray[lastSlotID].amoutText.text = inventoryItem.inventory.InventoryData.Data[lastSlotID].amount.ToString();
                        if (inventoryItem.inventory.InventoryData.Data[lastSlotID].amount >= inventoryItem.inventory.InventoryData.Data[lastSlotID].maxAmount) slots[lastSlotID].isFull = true;
                        Destroy(draggedItem.gameObject);
                    }

                }
            }

        }
        else
        {
            if (lastSlotID != -1)
            {
                if (!inventoryItem.inventory.slots[lastSlotID].isFill)
                {
                    //Debug.Log("Drop to an unavailable slot and come back to EMPTY slot");
                    inventoryItem.inventory.slots[lastSlotID].DropItem(draggedItem);
                    inventoryItem.inventory.InventoryData.Data[lastSlotID] = draggedItemData;
                    inventoryItem.inventory.InventoryItemsArray[lastSlotID] = draggedItem;
                }
                else if (!inventoryItem.inventory.slots[lastSlotID].isFull)
                {
                    //Debug.Log("Drop to an unavailable slot and come back to LAST and not EMPTY slot");
                    inventoryItem.inventory.InventoryData.Data[lastSlotID].amount += draggedItemData.amount;
                    inventoryItem.inventory.InventoryItemsArray[lastSlotID].amoutText.text = inventoryItem.inventory.InventoryData.Data[lastSlotID].amount.ToString();
                    if (inventoryItem.inventory.InventoryData.Data[lastSlotID].amount >= inventoryItem.inventory.InventoryData.Data[lastSlotID].maxAmount) slots[lastSlotID].isFull = true;
                    Destroy(draggedItem.gameObject);
                }

            }

        }

        inventoryItem.inventory.SetNullDraggedItem();
    }

    public void CombackToLastSlot(InventoryItem inventoryItem)
    {
        DropItem(inventoryItem.inventory.slots[inventoryItem.inventory.lastSlotID], inventoryItem.inventory.draggedItem);
    }

    public void SetNullDraggedItem()
    {
        draggedItem = null;
        draggedItemData = null;
        lastSlotID = -1;
    }

    public void ThrowItem()
    {
        OnThrowItem?.Invoke(draggedItemData);
        SetNullDraggedItem();
    }

    public void ShowInfo(int slotId)
    {
        inforBoard.ShowItemInfo(InventoryData.Data[slotId]);
    }

    public void SelectItem(int slotId)
    {
        OnSelectItem?.Invoke(InventoryData.Data[slotId]);
    }

    public bool IsContain(string itemName)
    {
        foreach(var item in InventoryData.Data)
            if(string.Compare(itemName,item.itemName) == 0) return true;
        return false;
    }

    #region Data
    private void SaveData(string data, string fileName)
    {
        string dataPath = $"{Application.persistentDataPath}/{fileName}.txt";

        File.WriteAllText(dataPath, data);
    }
    private string LoadData(string fileName)
    {
        string dataPath = $"{Application.persistentDataPath}/{fileName}.txt";

        if (File.Exists(dataPath))
            return File.ReadAllText(dataPath);
        else
            return "";
    }
    public void SaveInventoryData()
    {
        //Debug.Log(JsonUtility.ToJson(InventoryData));
        SaveData(JsonUtility.ToJson(InventoryData), "InventoryData");
    }
    public void LoadInventoryData()
    {
        string data = LoadData("InventoryData");

        if (data != "")
        {
            ClearInventory();

            InventoryData tempInventoryItemsListData = JsonUtility.FromJson<InventoryData>(data);

            foreach (var item in tempInventoryItemsListData.Data)
            {
                if (item == null || item.itemName == "")
                    continue;
                AddItemToInventory(item);
            }
        }
        else
        {
            InventoryData = new InventoryData(slots.Count);
        }
    }
    #endregion Data

}


[Serializable]
public struct InventoryData
{
    public Item[] Data;

    public InventoryData(int length)
    {
        Data = new Item[length];
    }
}


