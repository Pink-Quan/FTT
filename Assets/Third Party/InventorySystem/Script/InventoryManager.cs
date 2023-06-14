using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance
    {
        get 
        {
            if (_instance == null)
            {
                _instance = Instantiate(Resources.Load<InventoryManager>("InventoryManager"));
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance; 
        }
        private set
        {
            _instance = value;
        }
    }
    private static InventoryManager _instance;

    public ItemConfig itemConfig;

    public void AddItemToInventory(ItemType type,string name,int amount,Inventory inventory)
    {
        Item food = new Item(type, name);
        food = itemConfig.GetItemConfig(food);
        food.amount = amount;
       
        if (inventory.gameObject.activeSelf)
        {
            inventory.AddItemToInventory(food);
        }
        else
        {
            inventory.gameObject.SetActive(true);
            inventory.AddItemToInventory(food);
            inventory.gameObject.SetActive(false);
        }
    }

    public void AddItemToInventory(Item item, Inventory inventory)
    {
        if (inventory.gameObject.activeSelf)
        {
            inventory.AddItemToInventory(item);
        }
        else
        {
            inventory.gameObject.SetActive(true);
            inventory.AddItemToInventory(item);
            inventory.gameObject.SetActive(false);
        }
    }

}
