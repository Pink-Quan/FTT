using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private CinemachineVirtualCamera vCam;

    [SerializeField] private InteractableEntity toHouseDoor;

    [SerializeField] private Vector3 playerInsideHousePos;

    private CharacterController Ngan;
    private CharacterController Minh;
    private CharacterController Mai;
    private CharacterController Nam;
    private CharacterController Hung;
    private PlayerController player;

    private MainMapStartCampingTexts texts;
    public void Init(CharacterController Ngan, CharacterController Minh, CharacterController Mai, CharacterController Nam, CharacterController Hung, PlayerController player, MainMapManager mainMapManager)
    {
        texts = texts = Resources.Load<MainMapStartCampingTexts>($"Texts/MainMap/StartCamping/{PlayerPrefs.GetString("Language", "Eng")}");

        this.Ngan = Ngan;
        this.Minh = Minh;
        this.Mai = Mai;
        this.Nam = Nam;
        this.Hung = Hung;
        this.player = player;
        this.mainMapManager = mainMapManager;

        InitStartCamping();
        Invoke("FirstComunicateWithManager", 2);
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
        character.UpdateMoveAnimation();
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
        Minh.AddConversationToCharacter(texts.MinhCamping1);
        Mai.AddConversationToCharacter(texts.MaiCamping1);
        Hung.AddConversationToCharacter(texts.HungCamping1);
        Ngan.AddConversationToCharacter(texts.NganCamping1);
        Nam.AddConversationToCharacter(texts.NamCamping1);
        player.SetArrowPointer(piecesOfIron[0].transform);
    }

    public void AddKeyToPlayer()
    {
        InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "Key", 1, player.inventory);
    }

    public void AddMagnetToPlayer()
    {
        GameManager.instance.textBoard.ShowText(texts.getMagnet, EnablePlayerMoveAndUI);
        InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "Magnet", 1, player.inventory);
    }

    bool isStartDoMission1;
    int mission1Progress;
    bool isAskMinh;

    public void CleanPieceOfIron(InteractableEntity entity)
    {
        player.OffArrowPointer();
        if (player.curItem == null)
        {
            if (!isStartDoMission1)
            {
                DisablePlayerMoveAndUI();
                isStartDoMission1 = true;
                Minh.AddConversationToCharacter(texts.MinhGuideToTakeMagnet, () => isAskMinh = true);
                GameManager.instance.dialogueManager.StartDialogue(texts.cantDoFirstMission, MoveCamToMinh);
                return;
            }
            else if (!isAskMinh)
            {
                DisablePlayerMoveAndUI();
                GameManager.instance.dialogueManager.StartDialogue(texts.cantDoFirstMission, EnablePlayerMoveAndUI);
                return;
            }
            GameManager.instance.dialogueManager.StartDialogue(texts.needMagnet, EnablePlayerMoveAndUI);
            return;
        }
        else
        {
            if (string.Compare(player.curItem.itemName, "Magnet") != 0)
            {
                if (!isStartDoMission1)
                {
                    DisablePlayerMoveAndUI();
                    isStartDoMission1 = true;
                    Minh.AddConversationToCharacter(texts.MinhGuideToTakeMagnet, () => isAskMinh = true);
                    GameManager.instance.dialogueManager.StartDialogue(texts.cantDoFirstMission, MoveCamToMinh);
                    return;
                }
                else if (!isAskMinh)
                {
                    DisablePlayerMoveAndUI();
                    GameManager.instance.dialogueManager.StartDialogue(texts.cantDoFirstMission, EnablePlayerMoveAndUI);
                    return;
                }
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
        Ngan.AddConversationToCharacter(texts.annouchToNganDoneFirstMission, NganGiveNewMission);
        GameManager.instance.missionsManager.missions.Remove(texts.firstMission);
        GameManager.instance.missionsManager.missions.Add(texts.callNganStartCampingMission);
        GameManager.instance.dialogueManager.StartDialogue(texts.doneFirstMissons, () =>
        {
            GameManager.instance.missionsManager.ShowMissions(EnablePlayerMoveAndUI);
            player.SetArrowPointer(Ngan.transform);
        });
    }

    private void MoveCamToMinh()
    {
        var cam = vCam.GetCinemachineComponent<CinemachineTransposer>();
        DOVirtual.Vector3(cam.m_FollowOffset, cam.m_FollowOffset + Minh.transform.position - vCam.transform.position, 2, value =>
        {
            value.z = -10;
            cam.m_FollowOffset = value;
        });
        vCam.transform.DOMove(Minh.transform.position - new Vector3(0, 0, 10), 2);
        Invoke("MoveCamBackToLinh", 2);
    }

    private void MoveCamBackToLinh()
    {
        var cam = vCam.GetCinemachineComponent<CinemachineTransposer>();
        DOVirtual.Vector3(cam.m_FollowOffset, new Vector3(0, 0, -10), 2, value =>
        {
            value.z = -10;
            cam.m_FollowOffset = value;
        }).OnComplete(EnablePlayerMoveAndUI);
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

    private void NganGiveNewMission()
    {
        DisablePlayerMoveAndUI();

        charNeedToCall = new List<Transform>();
        charNeedToCall.Add(Minh.transform);
        charNeedToCall.Add(Nam.transform);
        charNeedToCall.Add(Hung.transform);

        GameManager.instance.textBoard.ShowText(texts.scecondStartMission, EnablePlayerMoveAndUI);
        Ngan.AddConversationToCharacter(texts.callNganSecondMission);
        Mai.AddConversationToCharacter(texts.callMaiSecondMission);
        Minh.AddConversationToCharacter(texts.callMinhSecondMission, () => CallPeople(Minh.transform));
        Hung.AddConversationToCharacter(texts.callHungSecondMission, () => CallPeople(Hung.transform));
        Nam.AddConversationToCharacter(texts.callNamSecondMission, () => CallPeople(Nam.transform));

        player.SetArrowPointer(Nam.transform);
    }
    int ppCalled;
    List<Transform> charNeedToCall;
    private void CallPeople(Transform character)
    {
        if (character != player.transform)
            if (charNeedToCall.Contains(character))
            {
                charNeedToCall.Remove(character);
                ppCalled++;
            }
        if (ppCalled == 3)
        {
            player.SetArrowPointer(toHouseDoor.transform);
            toHouseDoor.onInteract.AddListener(DoneMission2);
        }
        else
        {
            player.SetArrowPointer(charNeedToCall[0]);
        }
    }

    public void GetMagnetHint()
    {
        GameManager.instance.textBoard.ShowText(texts.getMagnetHint);
    }

    private void DoneMission2(InteractableEntity entity)
    {
        DisablePlayerMoveAndUI();
        player.anim.SetDirection(Vector2.left);
        toHouseDoor.onInteract.RemoveListener(DoneMission2);
        player.OffArrowPointer();
        GameManager.instance.transitions.Transition(1, 1, ConversationInsideHouse, SetCharacterInsideHouse);
    }

    private void SetCharacterInsideHouse()
    {
        player.transform.position = playerInsideHousePos;
        Nam.transform.position = playerInsideHousePos + Vector3.down;
        Mai.transform.position = playerInsideHousePos + Vector3.up;
        Hung.transform.position = playerInsideHousePos + Vector3.up * 2;
        Ngan.transform.position = playerInsideHousePos + Vector3.up * 0.7f / 2 + Vector3.left * 1.5f;
        Minh.transform.position = playerInsideHousePos + Vector3.down + Vector3.left * 1.5f;

        player.anim.SetDirection(Vector2.left);
        Nam.anim.SetDirection(Vector2.left);
        Hung.anim.SetDirection(Vector2.left);
        Mai.anim.SetDirection(Vector2.left);
        Ngan.anim.SetDirection(Vector2.right);
        Minh.anim.SetDirection(Vector2.right);

        Nam.transform.SetParent(houseFirstFloor, true);
        Hung.transform.SetParent(houseFirstFloor, true);
        Ngan.transform.SetParent(houseFirstFloor, true);
        Minh.transform.SetParent(houseFirstFloor, true);
        Mai.transform.SetParent(houseFirstFloor, true);
    }

    private void ConversationInsideHouse()
    {
        GameManager.instance.dialogueManager.StartDialogue(texts.insideHouseConversations, ChangeToDay2);
    }

    private void ChangeToDay2()
    {
        startCampingInteractableEntities.SetActive(false);
        PlayerPrefs.SetInt("Progress", (int)GameProgress.CampingDay2);
        GameManager.instance.transitions.Transition(1, 1, null, mainMapManager.InitCampingDay2);
    }
}
