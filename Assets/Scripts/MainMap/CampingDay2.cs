using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampingDay2 : MonoBehaviour
{
    [SerializeField] GameObject interactableEntityes;
    [SerializeField] GameObject gameObjects;

    [SerializeField] Vector3 HungStartPos;
    [SerializeField] Vector3 MaiStartPos;
    [SerializeField] Vector3 NganStartPos;
    [SerializeField] Vector3 MinhStartPos;
    [SerializeField] Vector3 NamStartPos;
    [SerializeField] Vector3 playerStartPos;

    [SerializeField] Transform firstMissionPoint;
    [SerializeField] Transform carParkMissionPoint;

    [SerializeField] Vector3 playerMission3AppearPos;
    [SerializeField] GameObject mission3ObjectsParent;

    [SerializeField] Vector3 finalDisscusPos;
    [SerializeField] GameObject mainMap;
    [SerializeField] GameObject firstFlooor;

    private CharacterController Minh;
    private CharacterController Nam;
    private CharacterController Hung;
    private CharacterController Ngan;
    private CharacterController Mai;
    private PlayerController player;

    private MainMapManager mainMapManager;
    private MainMapDay2Texts texts;

    [SerializeField] private List<InteractableEntity> interactTrees;

    public void Init(MainMapManager mainMapManager)
    {
        interactableEntityes.SetActive(true);

        texts = Resources.Load<MainMapDay2Texts>($"Texts/MainMap/Day2/{PlayerPrefs.GetString("Language", "Eng")}");

        this.mainMapManager = mainMapManager;

        mainMap.SetActive(true);
        firstFlooor.SetActive(false);

        Minh = mainMapManager.Minh;
        Nam = mainMapManager.Nam;
        Hung = mainMapManager.Hung;
        Ngan = mainMapManager.Ngan;
        Mai = mainMapManager.Mai;
        player = mainMapManager.player;

        player.DisableMoveAndUI();
        //Uncommand 2 line of code and comment AnouchMinhDoneMission3() to end debug
        Invoke(nameof(ShowFirstMission), 1);
        SetStartDay2Character();
        //AnouchMinhDoneMission3();
    }

    private void ShowFirstMission()
    {
        player.SetArrowPointer(firstMissionPoint);
        GameManager.instance.textBoard.ShowText(texts.firstMission, player.EnableMoveAndUI);
    }

    private void SetStartDay2Character()
    {
        StartCoroutine(FirstMissionCheck());

        SetCharacterPos(Minh, MinhStartPos, Vector2.up);
        SetCharacterPos(Ngan, NganStartPos, Vector2.right);
        SetCharacterPos(Hung, HungStartPos, Vector2.up);
        SetCharacterPos(Nam, NamStartPos, Vector2.down);
        SetCharacterPos(player, playerStartPos, Vector2.down);
        //SetCharacterPos(Mai, MaiStartPos, Vector2.left);
        Mai.gameObject.SetActive(false);

        Minh.AddConversationToCharacter(texts.MinhFirstConversation);
        Nam.AddConversationToCharacter(texts.NamFirstConversation);
        Hung.AddConversationToCharacter(texts.HungFirstConversation);
        Minh.AddConversationToCharacter(texts.MinhFirstConversation);
    }

    private IEnumerator FirstMissionCheck()
    {
        while (true)
        {
            yield return null;

            if (((Vector2)firstMissionPoint.position - (Vector2)player.transform.position).sqrMagnitude < 2f)
            {
                MonologueAboutFirstMissionAndCallPlayerBack();
                yield break;
            }
        }
    }

    private void MonologueAboutFirstMissionAndCallPlayerBack()
    {
        player.DisableMoveAndUI();
        player.OffArrowPointer();
        player.anim.SetMove(false);

        Mai.gameObject.SetActive(true);
        Mai.transform.SetParent(null);
        Mai.transform.position = player.transform.position + new Vector3(1, -1) * 10;

        GameManager.instance.dialogueManager.StartDialogue(texts.playerMonologueAboutFirstMisson, MaiApproachPlayer);

        void MaiApproachPlayer()
        {
            Mai.UpdateMoveAnimation();
            Mai.transform.DOMove(player.transform.position + Vector3.right, 5).OnComplete(MaiCallPlayerToMovoToCarPark);
        }

        void MaiCallPlayerToMovoToCarPark()
        {
            player.anim.SetDirection(Vector2.right);
            Mai.StopMove();
            Mai.anim.SetDirection(Vector2.left);
            GameManager.instance.dialogueManager.StartDialogue(texts.callPlayerBackToCar, MaiMoveOut);
        }

        void MaiMoveOut()
        {
            Mai.UpdateMoveAnimation();
            Mai.transform.DOMove(player.transform.position + new Vector3(1, -1) * 10, 5).OnComplete(InitToCarPark);
        }

        void InitToCarPark()
        {
            GameManager.instance.missionsManager.AddAndShowMission(texts.backToCarParkMisson, player.EnableMoveAndUI);
            Mai.gameObject.SetActive(false);
            player.SetArrowPointer(carParkMissionPoint);
            StartCoroutine(CheckMoveToCarParkMission());
        }
    }

    private IEnumerator CheckMoveToCarParkMission()
    {
        while (true)
        {
            if (((Vector2)carParkMissionPoint.position - (Vector2)player.transform.position).sqrMagnitude < 2f)
            {
                ConfessToPlayer();
                yield break;
            }
            yield return null;
        }
    }

    private void ConfessToPlayer()
    {
        player.DisableMoveAndUI();
        player.OffArrowPointer();

        GameManager.instance.missionsManager.RemoveMission(texts.backToCarParkMisson);
        GameManager.instance.transitions.Transition(1, 1, StartDialogueConfess, MoveCharaceterToCarPark);

        void MoveCharaceterToCarPark()
        {
            Mai.gameObject.SetActive(true);

            Vector3 playerPos = player.transform.position;


            Nam.gameObject.SetActive(true);
            Hung.gameObject.SetActive(true);
            Mai.gameObject.SetActive(true);
            Ngan.gameObject.SetActive(true);
            Minh.gameObject.SetActive(true);

            Mai.transform.SetParent(null, true);
            Hung.transform.SetParent(null, true);
            Ngan.transform.SetParent(null, true);
            Minh.transform.SetParent(null, true);

            Nam.transform.position = playerPos + Vector3.right;
            Hung.transform.position = playerPos + Vector3.down + Vector3.left;
            Mai.transform.position = playerPos + Vector3.down + Vector3.right * 2;
            Ngan.transform.position = playerPos + Vector3.down * 2;
            Minh.transform.position = playerPos + Vector3.down * 2 + Vector3.left;

            player.anim.SetDirection(Vector2.down);
            Nam.anim.SetDirection(Vector2.down);
            Mai.anim.SetDirection(Vector2.left);
            Hung.anim.SetDirection(Vector2.right);
            Ngan.anim.SetDirection(Vector2.up);
            Minh.anim.SetDirection(Vector2.up);
        }

        void StartDialogueConfess()
        {
            GameManager.instance.dialogueManager.StartDialogue(texts.confessTheTruth, InitMission3);
        }
    }

    private void SetCharacterPos(CharacterController character, Vector3 pos, Vector2 dir)
    {
        character.transform.position = pos;
        character.anim.SetDirection(dir);
    }

    private void InitMission3()
    {
        Debug.Log("Start Misssion 3");

        GameManager.instance.transitions.Transition(1, 1, NganGuidePlayerToDoMission3, SetCharacterPosition);

        InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "Magnet", 1, player.inventory);

        void SetCharacterPosition()
        {
            SetCharacterPos(player, playerMission3AppearPos, Vector2.right);
            SetCharacterPos(Ngan, playerMission3AppearPos + new Vector3(1.5f, 0), Vector2.left);
        }

        void NganGuidePlayerToDoMission3()
        {
            GameManager.instance.dialogueManager.StartDialogue(texts.NganGuidePlayerToDoMission3, StartMission3);
        }
    }

    private void StartMission3()
    {
        mission3ObjectsParent.SetActive(true);
        GameManager.instance.missionsManager.AddAndShowMission(texts.mission3Description, player.EnableMoveAndUI);
        player.SetArrowPointer(GetNearestInteractableEntity(interactTrees, player.transform).transform);
    }

    bool firtRealizeCantTakeNail;
    public void GetNailInTheTree(InteractableEntity entity)
    {
        bool isHoldMagnet = false;
        if (player.curItem != null)
            if (string.Compare(player.curItem.itemName, "Magnet") == 0)
                isHoldMagnet = true;


        if (!firtRealizeCantTakeNail && !isHoldMagnet)
        {
            player.DisableMoveAndUI();
            GameManager.instance.dialogueManager.StartDialogue(texts.talkToNganAboutNailInTree, player.EnableMoveAndUI);
            firtRealizeCantTakeNail = true;
            return;
        }

        if (isHoldMagnet)
        {
            interactTrees.Remove(entity);
            mainMapManager.PlayParticalEffect(0, entity.transform.position);
            entity.gameObject.SetActive(false);
            if (interactTrees.Count == 0)
            {
                mission3ObjectsParent.SetActive(false);
                player.SetArrowPointer(Ngan.transform);
                Ngan.AddConversationToCharacter(texts.anouchNganDoneMission3, AnouchMinhDoneMission3, HeadToPlayer);

                void HeadToPlayer()
                {
                    player.arrowPointer.gameObject.SetActive(false);
                    Ngan.anim.SetDirection((Vector2)(player.transform.position - Ngan.transform.position));
                }
            }
            else
            {
                player.SetArrowPointer(GetNearestInteractableEntity(interactTrees, player.transform).transform);
            }
        }
        else
        {
            player.DisableMoveAndUI();
            GameManager.instance.dialogueManager.StartDialogue(texts.playerMonodialogueCantTakeNail, player.EnableMoveAndUI);
        }
    }

    public InteractableEntity GetNearestInteractableEntity(List<InteractableEntity> interactables, Transform target)
    {
        InteractableEntity res = null;
        float minDix = float.MaxValue;
        foreach (var interactable in interactables)
        {
            var vec = target.position - interactable.transform.position;
            vec.z = 0;
            var dis = vec.sqrMagnitude;
            if (dis < minDix)
            {
                res = interactable;
                minDix = dis;
            }
        }
        return res;
    }

    private void AnouchMinhDoneMission3()
    {
        player.DisableMoveAndUI();

        GameManager.instance.transitions.Transition(1, 1, OnComplete: FinalDiscussion, OnStartStay: MoveAllCharToTheHouse);
        void MoveAllCharToTheHouse()
        {
            mainMap.SetActive(false);
            firstFlooor.SetActive(true);

            SetCharacterPositionInHouse();
        }
    }

    private void FinalDiscussion()
    {
        PlayerPrefs.SetInt("Progress", (int)GameProgress.CampingDay3);
        GameManager.instance.dialogueManager.StartDialogue(texts.endDay2Dialogue, mainMapManager.InitCampingDay3);
        GameManager.instance.dbManager.UpdateDB();
    }

    public void SetCharacterPositionInHouse(PlayerController tplayer = null, CharacterController tHung = null, CharacterController tMai = null,
        CharacterController tNgan = null, CharacterController tMinh = null, CharacterController tNam = null)
    {
        if (player == null) player = tplayer;
        if (Hung == null) Hung = tHung;
        if (Mai == null) Mai = tMai;
        if (Ngan == null) Ngan = tNgan;
        if (Minh == null) Minh = tMinh;
        if (Nam == null) Nam = tNam;

        player.transform.position = finalDisscusPos;
        Vector3 playerPos = player.transform.position;
        Hung.transform.position = playerPos + Vector3.down;
        Mai.transform.position = playerPos + Vector3.down * 2;
        Nam.transform.position = playerPos + Vector3.down * 3;
        Ngan.transform.position = playerPos + Vector3.left * 1.5f + Vector3.down;
        Minh.transform.position = playerPos + Vector3.down * 2 + Vector3.left * 1.5f;

        player.anim.SetDirection(Vector2.left);
        Nam.anim.SetDirection(Vector2.left);
        Mai.anim.SetDirection(Vector2.left);
        Hung.anim.SetDirection(Vector2.left);
        Ngan.anim.SetDirection(Vector2.right);
        Minh.anim.SetDirection(Vector2.right);
    }
}

