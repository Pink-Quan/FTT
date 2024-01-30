using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampingDay3 : MonoBehaviour
{
    [SerializeField] private GameObject gameObjects;
    [SerializeField] private GameObject mainHouse;
    [SerializeField] private GameObject mainMap;
    [SerializeField] private Vector3 playerFirstMissionPos;
    [SerializeField] private GameObject fullBlackUI;
    [SerializeField] private VectorPaths pathNganFirstMissionMoveOut;
    [SerializeField] private InteractableEntity to2ndFloor;
    [SerializeField] private InteractableEntity toCamp;
    [SerializeField] private Vector3 wakeupAfterFaintingPos = new Vector3(116.14f, -58.63f);

    [SerializeField] private GameObject dirtyDisks;
    [SerializeField] private GameObject foodsOnTable;
    [SerializeField] private InteractableEntity door;
    [SerializeField] private ChessGame chess;

    [Header("Band Ending")]
    [SerializeField] private Vector3[] allCharDiePos;
    [SerializeField] private GameObject charBloods;
    [SerializeField] private Transform secondFloor;
    [SerializeField] private CinemachineVirtualCamera cineCam;
    [SerializeField] private float killerDuration;

    private CharacterController Minh;
    private CharacterController Ngan;
    private CharacterController Mai;
    private CharacterController Hung;
    private CharacterController Nam;
    private CharacterController Killer;


    PlayerController player;
    MainMapManager mainMapManager;
    CampingDay3Text texts;

    public void Init()
    {
        mainMapManager = GetComponent<MainMapManager>();
        to2ndFloor.canInteract = false;
        Minh = mainMapManager.Minh;
        Ngan = mainMapManager.Ngan;
        Mai = mainMapManager.Mai;
        Hung = mainMapManager.Hung;
        Nam = mainMapManager.Nam;
        Killer = mainMapManager.Killer;

        texts = Resources.Load<CampingDay3Text>($"Texts/MainMap/Day3/{PlayerPrefs.GetString("Language", "Eng")}");
        player = GameManager.instance.player;
        player.DisableMoveAndUI();
        //GameManager.instance.transitions.Transition(1, 1, WakePlayerUp, MovePlayerToFirstMission);
        gameObjects.SetActive(true);
        toCamp.canInteract = false;

        //Debug
        MovePlayerToFirstMission();
        player.EnableMoveAndUI();
    }

    private void WakePlayerUp()
    {
        texts.wakePlayerUp[texts.wakePlayerUp.FindDiague("Yawn")].onStart = () =>
        {
            GameManager.instance.soundManager.PlaySound("Yawn");
        };
        texts.wakePlayerUp[texts.wakePlayerUp.FindDiague("Strangle Linh")].onStart = () =>
        {
            //GameManager.instance.soundManager.PlaySound("Being Strangled");
        };
        GameManager.instance.dialogueManager.StartDialogue(texts.wakePlayerUp, () =>
        {
            GameManager.instance.transitions.Transition(1, 0, NganGiveMissionToLinh, () => fullBlackUI.gameObject.SetActive(false));
        });
    }

    private void MovePlayerToFirstMission()
    {
        fullBlackUI.gameObject.SetActive(true);
        Mai.gameObject.SetActive(false);
        Nam.gameObject.SetActive(false);
        Minh.gameObject.SetActive(false);
        Hung.gameObject.SetActive(false);
        mainMap.SetActive(false);
        mainHouse.SetActive(true);
        Ngan.gameObject.SetActive(true);
        player.SetPositon(playerFirstMissionPos, Vector3.right);
        mainMapManager.Ngan.SetPositon(playerFirstMissionPos + Vector3.right, Vector3.left);
    }

    private void NganGiveMissionToLinh()
    {
        GameManager.instance.dialogueManager.StartDialogue(texts.playerFirstDialgue, StartFirstMission);
    }

    private void StartFirstMission()
    {
        Ngan.UpdateMoveAnimation();
        Ngan.transform.DOPath(pathNganFirstMissionMoveOut.paths, 1).SetEase(Ease.Linear).OnComplete(() =>
        {
            Ngan.StopMove();
            Ngan.gameObject.SetActive(false);
            GameManager.instance.missionsManager.AddAndShowMission(texts.firstMissionText, () =>
            {
                player.EnableMoveAndUI();
            });
        });
    }

    bool isDoneWashDisks;
    public void WashDishes(InteractableEntity entity)
    {
        if (isDoneWashDisks) return;
        isDoneWashDisks = true;
        player.DisableMoveAndUI();
        GameManager.instance.dialogueManager.StartDialogue(texts.monodialogueAboutDiryDisks, () =>
        {
            player.EnableMoveAndUI();
            mainMapManager.PlayParticalEffect(0, player.transform.position);
            dirtyDisks.SetActive(false);
            GameManager.instance.fastNotification.Show(player.transform.position + Vector3.up, texts.doneWashingDisksNotify);
        });
        CheckDoneFirstMissions();
        entity.gameObject.SetActive(false);
    }

    bool isDonePrepareFoods;
    bool isDoneCookingFoods;
    public void PrepareFoods(InteractableEntity entity)
    {
        entity.gameObject.SetActive(false);
        isDonePrepareFoods = true;
        GameManager.instance.fastNotification.Show(player.transform.position + Vector3.up, texts.prepareFoodNotify);
        mainMapManager.PlayParticalEffect(0, player.transform.position);
    }

    public void Cooking(InteractableEntity entity)
    {
        if (!isDonePrepareFoods)
        {
            GameManager.instance.fastNotification.Show(player.transform.position + Vector3.up, texts.needPrepareFoodInFridgeFirst);
            return;
        }
        foodsOnTable.SetActive(true);
        entity.gameObject.SetActive(false);
        isDonePrepareFoods = true;
        GameManager.instance.fastNotification.Show(player.transform.position + Vector3.up, texts.cookingFoodNotify);
        mainMapManager.PlayParticalEffect(0, player.transform.position);
        isDoneCookingFoods = true;
        CheckDoneFirstMissions();
    }

    private void CheckDoneFirstMissions()
    {
        if (!(isDoneCookingFoods && isDoneWashDisks)) return;
        door.canInteract = true;
        player.DisableMoveAndUI();
        GameManager.instance.dialogueManager.StartDialogue(texts.forgotCheckDoor, player.EnableMoveAndUI);
    }

    public void CheckDoorLock(InteractableEntity entity)
    {
        player.DisableMoveAndUI();
        player.anim.SetDirection(Vector2.down);
        GameManager.instance.dialogueManager.StartDialogue(texts.monoDialogueAboutUnlockDoor, () =>
        {
            player.EnableMoveAndUI();
            Invoke(nameof(ChessNotify), 5);
            GameManager.instance.soundManager.PlaySound("Lock Door");
            GameManager.instance.fastNotification.Show(player.transform.position + Vector3.up, texts.lockDoor);
        });
        entity.canInteract = false;
        entity.gameObject.SetActive(false);
    }

    private void SetPlayerLockDoorPos()
    {

    }

    private void ChessNotify()
    {
        player.DisableMoveAndUI();
        GameManager.instance.soundManager.PlaySound("PhoneVibration");
        GameManager.instance.dialogueManager.StartDialogue(texts.monoDialogueAboutPhone, () =>
        {
            player.EnableMoveAndUI();
            Invoke(nameof(RealizeChessFamiliar), 5);
        });
    }

    private void RealizeChessFamiliar()
    {
        player.DisableMoveAndUI();
        GameManager.instance.dialogueManager.StartDialogue(texts.monoDialogueAboutChess, () =>
        {
            player.EnableMoveAndUI();
            Invoke(nameof(FeelDizzyAfterLookingAtChess), 5);
        });
    }

    private void FeelDizzyAfterLookingAtChess()
    {
        player.DisableMoveAndUI();
        chess.outButton.onClick.AddListener(player.phone.outButton.onClick.Invoke);
        PlayerPrefs.SetInt("ProgressDay3", 1);
        player.onOpenPhone = () =>
        {
            chess.PlayChess();
            player.phone.gameObject.SetActive(false);
        };
        GameManager.instance.dialogueManager.StartDialogue(texts.fellDizzyAfterLookingAtChess, () =>
        {
            player.EnableMoveAndUI();
            player.stress.StartStress();
            player.anim.onDoneDie.AddListener(WakeUpAfterFainting);
            player.stress.onMaxStress.AddListener(player.Die);
        });
    }

    private void WakeUpAfterFainting()
    {
        player.anim.onDoneDie.RemoveListener(WakeUpAfterFainting);
        player.stress.StopBeingStress();
        player.stress.AddStress(-10);

        GameManager.instance.transitions.Transition(1, 1, OnComplete: () =>
        {
            GameManager.instance.dialogueManager.StartDialogue(texts.wakeUpAfterFainting, player.EnableMoveAndUI);

        },
        OnStartStay: () =>
        {
            player.Resurrect();
            player.SetPositon(wakeupAfterFaintingPos, Vector2.down);
            to2ndFloor.canInteract = true;
            toCamp.canInteract = true;
            if (chess.gameObject.activeSelf)
                chess.outButton?.onClick.Invoke();
            SetCharactersDie();
        });
    }

    private void SetCharactersDie()
    {
        secondFloor.gameObject.SetActive(true);
        charBloods.SetActive(true);

        SetCharDie(Ngan, allCharDiePos[0], 90);
        SetCharDie(Minh, allCharDiePos[1], 90);
        SetCharDie(Nam, allCharDiePos[2], 180);
        SetCharDie(Mai, allCharDiePos[3], -90);
        SetCharDie(Hung, allCharDiePos[4], -90);

        secondFloor.gameObject.SetActive(false);
    }

    private void SetCharDie(CharacterController character, Vector3 pos, float rot)
    {
        character.gameObject.SetActive(true);
        character.SetPositon(pos, Vector3.down);
        character.transform.rotation = Quaternion.Euler(0, 0, rot);
        character.anim.StopAnimation();
        character.transform.SetParent(secondFloor.transform, true);
    }

    public void SeeEveryoneDead(Collision2D collision,Collider2D caller)
    {
        player.DisableMoveAndUI();
        GameManager.instance.dialogueManager.StartDialogue(texts.seeEveryoneDead, BadEnding);
        caller.enabled = false;
    }

    private void BadEnding()
    {
        DOVirtual.Float(cineCam.m_Lens.OrthographicSize, 2.5f, 2, orthoSize =>
        {
            cineCam.m_Lens.OrthographicSize = orthoSize;
        }).OnComplete(ApproachPlayer);

        void ApproachPlayer()
        {
            Killer.gameObject.SetActive(true);
            Killer.UpdateMoveAnimation();
            Killer.col.enabled = false;
            Killer.SetPositon(player.transform.position + Vector3.up * 5, Vector3.down);
            Killer.transform.DOMoveY(player.transform.position.y, killerDuration).SetEase(Ease.OutQuart).OnComplete(() =>
            {
                Killer.StopMove();
                GameManager.instance.soundManager.PlaySound("Kill");
                player.ImmediateDie(0.1f,onDone:() =>
                {
                    fullBlackUI.gameObject.SetActive(true);
                });
            });
        }
    }

    public void ReplayGameAfterBadEnding()
    {

    }
}
