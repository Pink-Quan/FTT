using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LinhHouseManager : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject nam;
    [SerializeField] private GameObject fridge;

    [SerializeField] private Vector3 playerInBedPos;
    [SerializeField] private Vector3 NamNearBedPos;
    [SerializeField] private Vector3 playerBreathPos;

    [SerializeField] private GameObject hung;
    [SerializeField] private GameObject mai;
    [SerializeField] private Vector3 playerMeetFriendsPos;

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

        texts = Resources.Load<LinhHouseTexts>($"Texts/Linh House/{PlayerPrefs.GetString("Language", "Eng")}");

        InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "Phone", 1, player.inventory);
        InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "Citizen Identity Card", 1, player.inventory);
    }

    private void FirstSeftConversation()
    {
        GameManager.instance.dialogueManager.StartDialogue(texts.firstSeftDialogue, EnablePlayerMoveAndUI);
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
        GameManager.instance.dialogueManager.StartDialogue(texts.seftDialogueAbountCookBook, EnablePlayerMoveAndUI);
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
        GameManager.instance.dialogueManager.StartDialogue(texts.monologueAbountStick, EnablePlayerMoveAndUI);
    }


    bool isMonologueTheLocker;
    public void MonologueTheLocker()
    {
        if (!isMonologueTheLocker)
        {
            isMonologueTheLocker = true;
            GameManager.instance.dialogueManager.StartDialogue(texts.monologueAbountLocker, null);
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
            GameManager.instance.dialogueManager.StartDialogue(texts.thereSomthingUnderTheChair, EnablePlayerMoveAndUI);
        }
    }

    private void MonologueAboutTheLastPage()
    {
        GameManager.instance.dialogueManager.StartDialogue(texts.monologueAboutThePage, EnablePlayerMoveAndUI);
    }

    public void HandleAfterLoginFacebook()
    {
        DisablePlayerMoveAndUI();
        GameManager.instance.dialogueManager.StartDialogue(texts.monologueAfterOpenningFacebook, () =>
        {
            EnablePlayerMoveAndUI();
            Invoke("NamKnockTheDoor", 3);
        });
    }

    public void ReadingDocInLocker()
    {
        GameManager.instance.dialogueManager.StartDialogue(texts.readingDocInLocker, EnablePlayerMoveAndUI);
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
        player.anim.SetMove(false);
        GameManager.instance.dialogueManager.StartDialogue(texts.monodialogueSomebodyKnockTheDoor, EnablePlayerMoveAndUI);
    }

    public void MonoDialogueThereNoOneOutside()
    {
        GameManager.instance.dialogueManager.StartDialogue(texts.monodialogueThereNoOneOutside, () =>
        {
            nam.SetActive(true);
            EnablePlayerMoveAndUI();
        });
    }

    public void CommunicateWithNam()
    {
        GameManager.instance.dialogueManager.StartDialogue(texts.firstMeetNam, EnablePlayerMoveAndUI);
        fridge.SetActive(true);
    }

    public void GetFoodInFridge()
    {
        GameManager.instance.dialogueManager.StartDialogue(texts.seeFoodInFirdge, () =>
        {
            EnablePlayerMoveAndUI();
            Invoke("FeelDizzy", 2);
        });
    }

    private void FeelDizzy()
    {
        DisablePlayerMoveAndUI();
        player.anim.SetMove(false);
        GameManager.instance.dialogueManager.StartDialogue(texts.LinhFeelingNotGood, () =>
        {
            player.anim.Die();
            Invoke("GoToBed", player.anim.DieTime);
        });
    }

    private void GoToBed()
    {
        GameManager.instance.transitions.Transition(1, 1, NamTalkWithLinhWhenHerInBed, () =>
        {
            player.transform.position = playerInBedPos;
            nam.transform.position = NamNearBedPos;
            player.anim.ResetAnim();
            player.anim.SetDirection(Vector2.down);
            player.anim.StopAllCoroutines();
        });
    }

    private void NamTalkWithLinhWhenHerInBed()
    {
        GameManager.instance.dialogueManager.StartDialogue(texts.NamTalkWithLinhWhenHerInBed, GuidePlayerToBreath);
    }

    private void GuidePlayerToBreath()
    {
        player.transform.position = playerBreathPos;
        player.anim.ResetAnim();
        player.anim.SetDirection(Vector2.down);
        player.HideButtons();
        GameManager.instance.dialogueManager.StartDialogue(texts.NamGuideLinhToBreath, () =>
        {
            GameManager.instance.player.EnableMove();
            player.stress.AddStress(100);
            player.stress.ShowStressBar();
            player.stress.breath.StartBreath(CompleteBreath, 10);
        });
    }

    private void CompleteBreath(int score)
    {
        player.stress.AddStress(-100);
        player.HideButtons();
        player.DisableMove();
        GameManager.instance.dialogueManager.StartDialogue(texts.NamTalkAfterLinhDoneBreath, () =>
        {
            player.ShowUI();
            player.EnableMove();
            Invoke("TalkWithPhone", 5);
        });
    }

    private void TalkWithPhone()
    {
        DisablePlayerMoveAndUI();
        player.anim.SetMove(false);
        player.anim.StopAllCoroutines();
        player.DisableMove();
        player.HideButtons();
        GameManager.instance.dialogueManager.StartDialogue(texts.LinhCommucatateThroughPhone, () =>
        {
            GameManager.instance.transitions.Transition(1, 1, ComunicateWithFriends, MeetFriends);
        });
    }

    private void MeetFriends()
    {
        mai.SetActive(true);
        hung.SetActive(true);
        player.transform.position = playerMeetFriendsPos;
        player.anim.ResetAnim();
        player.anim.SetDirection(Vector2.left);
    }

    private void ComunicateWithFriends()
    {
        GameManager.instance.dialogueManager.StartDialogue(texts.friendsConversations, () =>
        {
            GameManager.instance.transitions.Transition(1, 1, null, GoToMainScene);
        });
    }

    private void GoToMainScene()
    {
        PlayerPrefs.SetInt("Progress", (int)GameProgress.StartCamping);
        SceneManager.LoadScene("Mainmap");
    }

}
