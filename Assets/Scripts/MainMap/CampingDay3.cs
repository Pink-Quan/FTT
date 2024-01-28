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

    [SerializeField] private GameObject dirtyDisks;
    [SerializeField] private GameObject foodsOnTable;
    [SerializeField] private InteractableEntity door;
    [SerializeField] private ChessGame chess;

    private CharacterController Minh;
    private CharacterController Ngan;
    private CharacterController Mai;
    private CharacterController Hung;
    private CharacterController Nam;


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
        player.onOpenPhone = () =>
        {
            chess.PlayChess();
            player.phone.gameObject.SetActive(false);
        };
        GameManager.instance.dialogueManager.StartDialogue(texts.fellDizzyAfterLookingAtChess, () =>
        {
            player.EnableMoveAndUI();
            player.stress.StartStress();
        });
    }
}
