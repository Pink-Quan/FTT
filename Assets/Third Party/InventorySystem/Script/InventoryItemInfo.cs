using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemInfo : MonoBehaviour
{
    public Image icon;
    public TMP_Text itemName;
    public TMP_Text itemDescription;

    private void OnEnable()
    {
        icon.sprite = null;
        itemName.text = "";
        itemDescription.text = "";
    }

    public void ShowItemInfo(Item item)
    {
        icon.sprite = item.icon;
        itemName.text = item.itemName;
        itemDescription.text = item.info;
    }
}
