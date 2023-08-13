using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCamping : MonoBehaviour
{
    [SerializeField] private Vector3 playerStartCampingPos;
    [SerializeField] private GameObject startCampingInteractableEntities;

    [SerializeField] private VectorPaths NganPath;
    [SerializeField] private VectorPaths MinhPath;
    [SerializeField] private VectorPaths MaiPath;
    [SerializeField] private VectorPaths NamPath;
    [SerializeField] private VectorPaths HungPath;

    [SerializeField] private Vector3 NganFirstMissionPos;
    [SerializeField] private Vector3 MinhFirstMissionPos;
    [SerializeField] private Vector3 MaiFirstMissionPos;
    [SerializeField] private Vector3 NamFirstMissionPos;
    [SerializeField] private Vector3 HungFirstMissionPos;

    [SerializeField] private Transform houseFirstFloor;

    private CharacterController Ngan;
    private CharacterController Minh;
    private CharacterController Mai;
    private CharacterController Nam;
    private CharacterController Hung;
    private PlayerController player;
    private MainMapTexts texts;
    public void Init(CharacterController Ngan, CharacterController Minh, CharacterController Mai, CharacterController Nam, CharacterController Hung, PlayerController player,MainMapTexts texts)
    {
        this.Ngan = Ngan;
        this.Minh = Minh;
        this.Mai = Mai;
        this.Nam = Nam;
        this.Hung = Hung;
        this.player = player;
        this.texts = texts;

        InitStartCamping();
        Invoke("FirstComunicateWithManager", 2);
        AddKeyToPlayer();
    }
    private void InitStartCamping()
    {
        player.DisableMove();
        player.HideUI();

        player.transform.position = playerStartCampingPos;
        Ngan.transform.position = playerStartCampingPos + Vector3.up * 2 + Vector3.left * 1.5f;
        Minh.transform.position = playerStartCampingPos + Vector3.up * 2 + Vector3.left / 2;
        Mai.transform.position = playerStartCampingPos + Vector3.left * 1.1f;
        Hung.transform.position = playerStartCampingPos + Vector3.left * 2 * 1.1f;
        Nam.transform.position = playerStartCampingPos + Vector3.right * 1.1f;

        player.anim.SetDirection(Vector3.up);
        Ngan.anim.SetDirection(Vector3.down);
        Minh.anim.SetDirection(Vector3.down);
        Mai.anim.SetDirection(Vector3.up);
        Hung.anim.SetDirection(Vector3.up);
        Nam.anim.SetDirection(Vector3.up);

        startCampingInteractableEntities.gameObject.SetActive(true);
    }

    private void FirstComunicateWithManager()
    {
        GameManager.instance.dialogueManager.StartDialogue(texts.theFirstConversationsWithManagers, CharacterMove);
    }

    private void CharacterMove()
    {
        Invoke("PlayerStartFirstMission", 5);
        CharacterLeaveToFirstMission(Ngan, NganPath, NganFirstMissionPos);
        CharacterLeaveToFirstMission(Minh, MinhPath, MinhFirstMissionPos,dir: Vector2.up);
        CharacterLeaveToFirstMission(Mai, MaiPath, MaiFirstMissionPos,houseFirstFloor);
        CharacterLeaveToFirstMission(Nam, NamPath, NamFirstMissionPos);
        CharacterLeaveToFirstMission(Hung, HungPath, HungFirstMissionPos);
    }

    private void CharacterLeaveToFirstMission(CharacterController character,VectorPaths path,Vector3 point,Transform parent=null,Vector2 dir=new Vector2())
    {
        character.StartUpdateMove();
        character.transform.DOPath(path.paths, 5).OnComplete(() => 
        {
            character.StopMove();
            character.transform.position = point;
            if (parent != null)
            {
                character.transform.SetParent(parent);
            }
            if(dir!=Vector2.zero)
            {
                character.anim.SetDirection(dir);
            }
        });
    }

    private void PlayerStartFirstMission()
    {
        GameManager.instance.missionsManager.missions.Add(texts.firstMission);
        GameManager.instance.missionsManager.ShowMissions(() =>
        {
            player.EnableMove();
            player.ShowUI();
        });

        AddConversationToCharacter(Minh, texts.MinhCamping1);
        AddConversationToCharacter(Mai, texts.MaiCamping1);
        AddConversationToCharacter(Hung, texts.HungCamping1);
        AddConversationToCharacter(Ngan, texts.NganCamping1);
        AddConversationToCharacter(Nam, texts.NamCamping1);
    }

    public void AddKeyToPlayer()
    {
        InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "Key", 1, player.inventory);
    }

    private void AddConversationToCharacter(CharacterController character,Dialogue dialogue)
    {
        player.HideUI();
        player.DisableMove();
        var onInteract = character.interact.OnInteract;
        character.interact.canInteract = true;
        onInteract.RemoveAllListeners();
        onInteract.AddListener(entity =>
        {
            GameManager.instance.dialogueManager.StartDialogue(dialogue, () =>
            {
                player.ShowUI();
                player.EnableMove();
            });
        });
    }
}
