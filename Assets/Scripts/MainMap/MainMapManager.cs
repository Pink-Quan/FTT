using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMapManager : MonoBehaviour
{
    public CharacterController Ngan;
    public CharacterController Minh;
    public CharacterController Mai;
    public CharacterController Nam;
    public CharacterController Hung;
    public CharacterController Killer;
    public PlayerController player;

    public ShowImage showImage;

    [SerializeField] private ParticleSystem[] effs;
    [SerializeField] private StartCamping startCamping;
    [SerializeField] private CampingDay2 campingDay2;
    [SerializeField] private CampingDay3 campingDay3;
    [SerializeField] private CampingDay4 campingDay4;

    private void Start()
    {
        Mai.gameObject.SetActive(false);
        Minh.gameObject.SetActive(false);
        Ngan.gameObject.SetActive(false);
        Nam.gameObject.SetActive(false);
        Hung.gameObject.SetActive(false);
        Killer.gameObject.SetActive(false);

        InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "Phone", 1, player.inventory);
        Debug.Log("Progress: " + (GameProgress)PlayerPrefs.GetInt("Progress"));
        switch ((GameProgress)PlayerPrefs.GetInt("Progress"))
        {
            case GameProgress.StartCamping:
                InitStartCamping();
                break;
            case GameProgress.CampingDay2:
                InitCampingDay2();
                break;
            case GameProgress.CampingDay3:
                InitCampingDay3();
                break;
            case GameProgress.CampingDay4:
                InitCampingDay4();
                break;
        }
    }

    public void AddConversationToCharacter(CharacterController character, Dialogue dialogue, Action onDone = null)
    {
        var onInteract = character.interact.onInteract;
        character.interact.canInteract = true;
        onInteract.RemoveAllListeners();
        onInteract.AddListener(TalkWithCharacter);

        void TalkWithCharacter(InteractableEntity entity)
        {
            DisablePlayerMoveAndUI();
            GameManager.instance.dialogueManager.StartDialogue(dialogue, DoneTalkWithCharacter);

            void DoneTalkWithCharacter()
            {
                EnablePlayerMoveAndUI();
                player.ShowInteractButton();
            }

            onDone?.Invoke();
        }
    }
    public void AddConversationToCharacter(CharacterController character, Dialogue[] dialogue, Action onDone = null)
    {
        var onInteract = character.interact.onInteract;
        character.interact.canInteract = true;
        onInteract.RemoveAllListeners();
        onInteract.AddListener(TalkWithCharacter);

        void TalkWithCharacter(InteractableEntity entity)
        {
            DisablePlayerMoveAndUI();
            GameManager.instance.dialogueManager.StartDialogue(dialogue, DoneTalkWithCharacter);

            void DoneTalkWithCharacter()
            {
                EnablePlayerMoveAndUI();
                player.ShowInteractButton();
            }

            onDone?.Invoke();
        }
    }

    public void PlayParticalEffect(int index, Vector3 pos)
    {
        effs[index].transform.position = pos;
        effs[index].Play();
        GameManager.instance.soundManager.PlayCommondSound("Get Item");
    }

    public void DisablePlayerMoveAndUI()
    {
        player.DisableMove();
        player.HideUI();
    }

    public void EnablePlayerMoveAndUI()
    {
        player.EnableMove();
        player.ShowUI();
    }

    public void InitStartCamping()
    {
        startCamping.Init(Ngan, Minh, Mai, Nam, Hung, player, this);
    }

    public void InitCampingDay2()
    {
        campingDay2.Init(this);
    }

    public void InitCampingDay3()
    {
        campingDay3.Init();
    }

    public void InitCampingDay4()
    {
        campingDay4.Init(this);
    }

    public void ChangeToCaveScene()
    {
        print("Change to cave");
    }
}
