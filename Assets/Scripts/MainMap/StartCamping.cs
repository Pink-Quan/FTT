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
    [SerializeField] private MainMapManager mainMapManager;

    [SerializeField] private InteractableEntity[] piecesOfIron;

    private CharacterController Ngan;
    private CharacterController Minh;
    private CharacterController Mai;
    private CharacterController Nam;
    private CharacterController Hung;
    private PlayerController player;
    private MainMapTexts texts;
    public void Init(CharacterController Ngan, CharacterController Minh, CharacterController Mai, CharacterController Nam, CharacterController Hung, PlayerController player, MainMapTexts texts, MainMapManager mainMapManager)
    {
        this.Ngan = Ngan;
        this.Minh = Minh;
        this.Mai = Mai;
        this.Nam = Nam;
        this.Hung = Hung;
        this.player = player;
        this.texts = texts;
        this.mainMapManager = mainMapManager;

        InitStartCamping();
        Invoke("FirstComunicateWithManager", 2);
        AddKeyToPlayer();
    }
    private void InitStartCamping()
    {
        DisablePlayerMoveAndUI();
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
        CharacterLeaveToFirstMission(Minh, MinhPath, MinhFirstMissionPos, dir: Vector2.up);
        CharacterLeaveToFirstMission(Mai, MaiPath, MaiFirstMissionPos, houseFirstFloor);
        CharacterLeaveToFirstMission(Nam, NamPath, NamFirstMissionPos);
        CharacterLeaveToFirstMission(Hung, HungPath, HungFirstMissionPos);
    }

    private void CharacterLeaveToFirstMission(CharacterController character, VectorPaths path, Vector3 point, Transform parent = null, Vector2 dir = new Vector2())
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
            if (dir != Vector2.zero)
            {
                character.anim.SetDirection(dir);
            }
        });
    }

    private void PlayerStartFirstMission()
    {
        GameManager.instance.missionsManager.missions.Add(texts.firstMission);
        GameManager.instance.missionsManager.ShowMissions(EnablePlayerMoveAndUI);

        mainMapManager.AddConversationToCharacter(Minh, texts.MinhCamping1);
        mainMapManager.AddConversationToCharacter(Mai, texts.MaiCamping1);
        mainMapManager.AddConversationToCharacter(Hung, texts.HungCamping1);
        mainMapManager.AddConversationToCharacter(Ngan, texts.NganCamping1);
        mainMapManager.AddConversationToCharacter(Nam, texts.NamCamping1);
    }

    public void AddKeyToPlayer()
    {
        InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "Key", 1, player.inventory);
    }

    public void AddMagnetToPlayer()
    {
        isGetMagnet = true;
        GameManager.instance.textBoard.ShowText(texts.getMagnet, EnablePlayerMoveAndUI);
        InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "Magnet", 1, player.inventory);
    }

    bool isGetMagnet;
    bool isStartDoMission1;
    int mission1Progress;
    bool isAskMinh;

    public void CleanPieceOfIron(InteractableEntity entity)
    {
        if (!isStartDoMission1)
        {
            DisablePlayerMoveAndUI();
            isStartDoMission1 = true;
            mainMapManager.AddConversationToCharacter(Minh, texts.MinhGuideToTakeMagnet, () => isAskMinh = true);
            GameManager.instance.dialogueManager.StartDialogue(texts.cantDoFirstMission, EnablePlayerMoveAndUI);
            return;
        }
        else if (isAskMinh || !isGetMagnet)
        {
            DisablePlayerMoveAndUI();
            GameManager.instance.dialogueManager.StartDialogue(texts.cantDoFirstMission, EnablePlayerMoveAndUI);
            return;
        }
        else
        {
            if (player.curItem == null)
            {
                GameManager.instance.dialogueManager.StartDialogue(texts.needMagnet, EnablePlayerMoveAndUI);
                return;
            }
            else if (string.Compare(player.curItem.itemName, "Magnet") != 0)
            {
                GameManager.instance.dialogueManager.StartDialogue(texts.needMagnet, EnablePlayerMoveAndUI);
                return;
            }
        }


        mainMapManager.PlayParticalEffect(0, entity.transform.position);
        entity.HideInteractButton();
        entity.gameObject.SetActive(false);
        mission1Progress++;
        if (mission1Progress < piecesOfIron.Length) return;
        DisablePlayerMoveAndUI();
        GameManager.instance.missionsManager.missions.Remove(texts.firstMission);
        GameManager.instance.missionsManager.missions.Add(texts.scecondStartMission);
        GameManager.instance.dialogueManager.StartDialogue(texts.doneFirstMissons, () =>
        {
            GameManager.instance.missionsManager.ShowMissions(EnablePlayerMoveAndUI);
        });
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
}
