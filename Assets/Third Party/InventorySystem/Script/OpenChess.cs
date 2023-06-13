using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChess : MonoBehaviour
{
    public Inventory targetInventory;

    public void OpenChest()
    {
        gameObject.SetActive(true);
        targetInventory.gameObject.SetActive(true);
        targetInventory.buttons.SetActive(false);
        targetInventory.board.anchoredPosition3D -= Vector3.up * 137;
    }

    public void CloseChest()
    {
        targetInventory.board.anchoredPosition3D += Vector3.up * 137;
        targetInventory.buttons.SetActive(true);
        targetInventory.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
