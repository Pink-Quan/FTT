using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinhHouseManager : MonoBehaviour
{
    [SerializeField] private GameObject door;

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
        GameManager.instance.dialogManager.StartDialogue(texts.firstSeftDialogue, EnablePlayerMoveAndUI);
    }

    private void EnablePlayerMoveAndUI()
    {
        player.ShowButtons();
        player.EnableMove();
    }

    public void AddCookBookToPlayer()
    {
        DisablePlayerMoveAndUI();
        InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "Cookery book", 1, player.inventory);
        GameManager.instance.textBoard.ShowText(texts.getCookBookGuide, SeftDiaglogueAboutCookBook);
    }
    private void SeftDiaglogueAboutCookBook()
    {
        GameManager.instance.dialogManager.StartDialogue(texts.seftDialogueAbountCookBook, EnablePlayerMoveAndUI);
    }
    public void DisablePlayerMoveAndUI()
    {
        player.DisableMove();
        player.HideButtons();
    }

    public void AddStickToPlayer()
    {
        DisablePlayerMoveAndUI();
        InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "stick", 1, player.inventory);
        GameManager.instance.textBoard.ShowText(texts.getStick, MonologueAbountStick);
    }

    private void MonologueAbountStick()
    {
        GameManager.instance.dialogManager.StartDialogue(texts.monologueAbountStick, EnablePlayerMoveAndUI);
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

    public void GetLastPage(InteractableEntity entity)
    {
        DisablePlayerMoveAndUI();
        if (player.curItem.itemName == "stick")
        {
            GameManager.instance.textBoard.ShowText(texts.getLastPage, MonologueAboutTheLastPage);
            InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "Last page of cookery book", 1, player.inventory);
            entity.gameObject.SetActive(false);
        }
        else
        {
            GameManager.instance.dialogManager.StartDialogue(texts.thereSomthingUnderTheChair, EnablePlayerMoveAndUI);
        }
    }

    private void MonologueAboutTheLastPage()
    {
        GameManager.instance.dialogManager.StartDialogue(texts.monologueAboutThePage, EnablePlayerMoveAndUI);
    }

    public void HandleAfterLoginFacebook()
    {
        DisablePlayerMoveAndUI();
        GameManager.instance.dialogManager.StartSequanceDialogue(texts.monologueAfterOpenningFacebook, () =>
        {
            EnablePlayerMoveAndUI();
            Invoke("NamKnockTheDoor", 3);
        });
    }

    private void NamKnockTheDoor()
    {
        DisablePlayerMoveAndUI();
        // Play knock sound
        Invoke("MonodialogueSomebodyKnockTheDoor", 2);
    }

    private void MonodialogueSomebodyKnockTheDoor()
    {
        door.SetActive(true);
        GameManager.instance.dialogManager.StartDialogue(texts.monodialogueSomebodyKnockTheDoor, EnablePlayerMoveAndUI);
    }

    public void MonoDialogueThereNoOneOutside()
    {
        GameManager.instance.dialogManager.StartDialogue(texts.monodialogueThereNoOneOutside, EnablePlayerMoveAndUI);
    }
}
