using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tes : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    void Start()
    {
        InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "Phone", 1, player.inventory);    
        //player.Die();
    }

    public void ShowFastNotification(string text)
    {
        GameManager.instance.fastNotification.Show(player.transform.position + Vector3.up, text);
    }
}
