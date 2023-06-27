using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinhHouseManager : MonoBehaviour
{
    private PlayerController player;
    private LinhHouseTexts texts;
    private void Start()
    {
        player = GameManager.instance.player;
        player.DisableMove();
        player.HideButtons();
        player.stress.HideStressBar();
        player.stress.HideBreathButton();
        Invoke("FirstSeftConversation", 2);

        texts = Resources.Load<LinhHouseTexts>("Texts/Linh House/Viet");

        InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "Phone", 1, player.inventory);
        InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "Citizen Identity Card", 1, player.inventory);
    }

    private void FirstSeftConversation()
    {
        GameManager.instance.dialogManager.StartDialogue(texts.firstSeftDialogue, PlayerExploreHouse);
    }

    private void PlayerExploreHouse()
    {
        player.ShowButtons();
        player.EnableMove();
    }

    public void AddCookBookToPlayer()
    {
        DisableMoveAndUI();
        InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "Cookery book", 1, player.inventory);
        GameManager.instance.textBoard.ShowText(texts.getCookBookGuide, SeftDiaglogueAboutCookBook);
    }
    private void SeftDiaglogueAboutCookBook()
    {
        GameManager.instance.dialogManager.StartDialogue(texts.seftDialogueAbountCookBook, PlayerExploreHouse);
    }
    public void DisableMoveAndUI()
    {
        player.DisableMove();
        player.HideButtons();
    }

    public void AddStickToPlayer()
    {
        DisableMoveAndUI();
        InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "stick", 1, player.inventory);
        GameManager.instance.textBoard.ShowText(texts.getStick, MonologueAbountStick);
    }

    private void MonologueAbountStick()
    {
        GameManager.instance.dialogManager.StartDialogue(texts.monologueAbountStick,PlayerExploreHouse);
    }


    bool isMonologueTheLocker;
    public void MonologueTheLocker()
    {
        if (!isMonologueTheLocker)
        {
            isMonologueTheLocker = true;
            GameManager.instance.dialogManager.StartDialogue(texts.monologueAbountLocker, null);
        }
    }

    public void GetLastPage()
    {
        // if grabbing stick, get papaer
        // else, show dialogue
    }
}
