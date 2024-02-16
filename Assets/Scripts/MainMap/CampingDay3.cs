using Chess.Game;
using Cinemachine;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
    [SerializeField] private GameObject doc;

    [SerializeField] private GameObject dirtyDisks;
    [SerializeField] private GameObject foodInFridge;
    [SerializeField] private GameObject cookingFood;
    [SerializeField] private GameObject foodsOnTable;
    [SerializeField] private InteractableEntity door;
    [SerializeField] private GameObject checkWindownInteractEntity;
    [SerializeField] private ChessGame chess;

    [Header("Bad Ending")]
    [SerializeField] private Vector3[] allCharDiePos;
    [SerializeField] private GameObject charBloods;
    [SerializeField] private Transform secondFloor;
    [SerializeField] private CinemachineVirtualCamera cineCam;
    [SerializeField] private float killerDuration;
    [SerializeField] private GameObject badEndingBoard;
    [SerializeField] private TMP_Text badEndingText;
    [SerializeField] private Vector3 replayLockDoorPos;
    [SerializeField] private GameObject wcMission;

    [Header("Not Bad Ending")]
    [SerializeField] private Vector3 playerInWcPos;
    [SerializeField] private GameObject wrongWC;
    [SerializeField] private VectorPaths killerReachPlayerInWcPath;

    [Header("Second Mission")]
    [SerializeField] private Vector3 MinhSecondMissionPos;
    [SerializeField] private Vector3 NganSecondMissionPos;
    [SerializeField] private Vector3 HungSecondMissionPos;
    [SerializeField] private Vector3 MaiSecondMissionPos;
    [SerializeField] private Vector3 NamSecondMissionPos;
    [SerializeField] private Vector3 playerSecondMissionPos;
    [SerializeField] private GameObject smartElectricController;

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
        Minh = mainMapManager.Minh;
        Ngan = mainMapManager.Ngan;
        Mai = mainMapManager.Mai;
        Hung = mainMapManager.Hung;
        Nam = mainMapManager.Nam;
        Killer = mainMapManager.Killer;

        texts = Resources.Load<CampingDay3Text>($"Texts/MainMap/Day3/{PlayerPrefs.GetString("Language", "Eng")}");
        player = GameManager.instance.player;
        player.DisableMoveAndUI();

        gameObjects.SetActive(true);
        toCamp.canInteract = false;
        to2ndFloor.canInteract = false;
        doc.SetActive(true);

        mainMap.SetActive(false);
        mainHouse.SetActive(true);

        switch (PlayerPrefs.GetInt("ProgressDay3", 0))
        {
            case 0:
                GameManager.instance.transitions.Transition(1, 1, WakePlayerUp, MovePlayerToFirstMission);
                break;
            case 1:
                GameManager.instance.transitions.Transition(1, 1, ReplayAfterLockDoor, InitPlayerAfterLockDoor);
                break;
            case 2:
                GameManager.instance.transitions.Transition(1, 1, () => WashFaceAndBrushTeeth(null), MovePlayerToWc);
                break;
        }
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
        checkWindownInteractEntity.SetActive(true);
        GameManager.instance.missionsManager.RemoveMission(texts.firstMissionText);
    }

    public void CheckWindow(InteractableEntity entity)
    {
        player.DisableMoveAndUI();
        entity.canInteract = false;
        entity.gameObject.SetActive(false);
        GameManager.instance.dialogueManager.StartDialogue(texts.checkWindow, player.EnableMoveAndUI);
    }

    public void CheckDoorLock(InteractableEntity entity)
    {
        checkWindownInteractEntity.SetActive(false);
        player.DisableMoveAndUI();
        player.anim.SetDirection(Vector2.down);
        GameManager.instance.dialogueManager.StartDialogue(texts.monoDialogueAboutUnlockDoor, () =>
        {
            player.EnableMoveAndUI();
            Invoke(nameof(ChessNotify), 5);
            GameManager.instance.soundManager.PlaySound("Lock Door");
            GameManager.instance.fastNotification.Show(player.transform.position + Vector3.up, texts.lockDoor);
        });
        if (entity != null)
        {
            entity.canInteract = false;
            entity.gameObject.SetActive(false);
        }
    }

    private void InitPlayerAfterLockDoor()
    {
        player.SetPositon(replayLockDoorPos, Vector2.down);
    }

    private void ReplayAfterLockDoor()
    {
        CheckDoorLock(null);
        foodsOnTable.SetActive(true);
        dirtyDisks.SetActive(false);
        foodInFridge.SetActive(false);
        cookingFood.SetActive(false);
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
        PlayerPrefs.SetInt("Died at campsite", 1);
        player.onOpenPhone += PlayChess;
        chess.onChessGameDone.AddListener(WinChess);
        GameManager.instance.dialogueManager.StartDialogue(texts.fellDizzyAfterLookingAtChess, () =>
        {
            player.EnableMoveAndUI();
            player.stress.StartStress();
            player.anim.onDoneDie.AddListener(WakeUpAfterFainting);
            player.stress.onMaxStress.AddListener(player.Die);
        });
    }

    private void PlayChess()
    {
        chess.PlayChess();
        player.phone.gameObject.SetActive(false);
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

    public void SeeEveryoneDead(Collision2D collision, Collider2D caller)
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
                GameManager.instance.CineCamShake(cineCam, 2, 0.2f);
                player.ImmediateDie(0.1f, onDone: () =>
                {
                    badEndingBoard.gameObject.SetActive(true);
                    badEndingText.text = texts.badEnding;
                });
            });
        }
    }

    public void ReplayGameAfterBadEnding()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void WinChess(string moves, GameResult.Result res)
    {
        if (res != GameResult.Result.BlackIsMated) return;

        player.onOpenPhone -= PlayChess;
        chess.onChessGameDone.RemoveListener(WinChess);
        chess.outButton?.onClick.Invoke();

        player.stress.onMaxStress.RemoveAllListeners();
        player.anim.onDoneDie.RemoveAllListeners();
        player.stress.StopBeingStress();

        PlayerPrefs.SetInt("ProgressDay3", 2);
        GameManager.instance.dialogueManager.StartDialogue(texts.playerWinChess, () =>
        {
            player.EnableMoveAndUI();
            wcMission.SetActive(true);
            wrongWC.SetActive(true);
        });
    }

    private void MovePlayerToWc()
    {
        player.SetPositon(playerInWcPos, Vector2.up);

    }

    public void WrongWC(InteractableEntity entity)
    {
        player.DisableMoveAndUI();
        GameManager.instance.dialogueManager.StartDialogue(texts.wrongWC, player.EnableMoveAndUI);
    }

    public void WashFaceAndBrushTeeth(InteractableEntity entity)
    {
        if (entity != null)
        {
            entity.canInteract = false;
            entity.gameObject.SetActive(false);
        }
        GameManager.instance.dialogueManager.StartDialogue(texts.washFaceAndBrushTeeth, KillerReachPlayerInWC);
    }

    private void KillerReachPlayerInWC()
    {
        DOVirtual.Float(cineCam.m_Lens.OrthographicSize, 2.5f, 2, value =>
        {
            cineCam.m_Lens.OrthographicSize = value;
        }).OnComplete(() =>
        {
            Killer.gameObject.SetActive(true);
            Killer.UpdateMoveAnimation();
            killerReachPlayerInWcPath.PushBackPoint(player.transform.position - new Vector3(0, 0.5f));
            Killer.transform.position = killerReachPlayerInWcPath.paths[0];
            Killer.transform.DOPath(killerReachPlayerInWcPath.paths, 1).OnComplete(() =>
            {
                //Comunitacte with player
                Killer.StopMove();
                Killer.anim.SetDirection(Vector2.up);
                GameManager.instance.CineCamShake(cineCam, 2, 0.2f);
                mainMapManager.showImage.Show(Resources.Load<Sprite>("CampingDay3/killerReachPlayer"));
                mainMapManager.showImage.ResizeImage();
                mainMapManager.showImage.ScaleImage(new Vector2(.7f, .7f));

                mainMapManager.showImage.image.color = new Color(1, 1, 1, 0);
                mainMapManager.showImage.image.DOFade(1, 1).OnComplete(() =>
                {
                    GameManager.instance.dialogueManager.StartDialogue(texts.killerTalkWithPlayer, KillerMoveOut);
                });
            }).SetEase(Ease.InCirc);
        });
    }

    private void KillerMoveOut()
    {
        fullBlackUI.gameObject.SetActive(false);
        mainMapManager.showImage.canvasGroup.DOFade(0, 1).OnComplete(() =>
        {
            mainMapManager.showImage.OnCloseViewImage();
            Killer.UpdateMoveAnimation();
            Array.Reverse(killerReachPlayerInWcPath.paths);
            Killer.transform.DOPath(killerReachPlayerInWcPath.paths, 2).OnComplete(OnKllerLeave).SetEase(Ease.OutCirc);
        });

    }

    private void OnKllerLeave()
    {
        Killer.StopMove();
        Killer.gameObject.SetActive(false);
        player.anim.SetDirection(Vector2.down);

        DOVirtual.Float(cineCam.m_Lens.OrthographicSize, 6, 2, value =>
        {
            cineCam.m_Lens.OrthographicSize = value;
        }).OnComplete(() =>
        {
            GameManager.instance.transitions.Transition(1, 1, CommunicateAfterEveryoneGetUp, InitAllCharMeetInLivingRoom);
        });
    }

    private void InitAllCharMeetInLivingRoom()
    {
        foodsOnTable.gameObject.SetActive(false);
        InitCharInHouse(Hung);
        InitCharInHouse(Mai);
        InitCharInHouse(Minh);
        InitCharInHouse(Ngan);
        InitCharInHouse(Nam);
        GetComponent<CampingDay2>().SetCharacterPositionInHouse(player, Hung, Mai, Ngan, Minh, Nam);
        mainMap.gameObject.SetActive(false);
        mainHouse.gameObject.SetActive(true);
    }

    private void InitCharInHouse(CharacterController character)
    {
        character.gameObject.SetActive(true);
        character.transform.SetParent(null, true);
    }

    private void CommunicateAfterEveryoneGetUp()
    {
        GameManager.instance.dialogueManager.StartDialogue(texts.firstAllCharConversation, InitSecondMission);
    }

    private void InitSecondMission()
    {
        GameManager.instance.transitions.Transition(1, 1, HungAskLinhAboutElectric, InitChar);
        smartElectricController.SetActive(true);
        toCamp.canInteract = true;
        to2ndFloor.canInteract = true;

        void InitChar()
        {
            InitCharSecondMission(player, playerSecondMissionPos, Vector2.down, null);
            InitCharSecondMission(Hung, HungSecondMissionPos, Vector2.down, mainMap.transform, OnCommunicateWithHung);
            InitCharSecondMission(Mai, MaiSecondMissionPos, Vector2.down, mainHouse.transform, OnCommunicateWithMai);
            InitCharSecondMission(Ngan, NganSecondMissionPos, Vector2.up, mainMap.transform, OnCommunicateWithNgan);
            InitCharSecondMission(Minh, MinhSecondMissionPos, Vector2.up, mainMap.transform, OnCommunicateWithMinh);
            Nam.gameObject.SetActive(false);
            Hung.interact.Radius = 2;
            Ngan.interact.Radius = 1;
        }
    }

    void OnCommunicateWithHung(InteractableEntity entity)
    {
        player.DisableMoveAndUI();
        GameManager.instance.dialogueManager.StartDialogue(texts.communicateWithHung2, player.EnableMoveAndUI);
        Minh.interact.onInteract.RemoveAllListeners();
        Minh.interact.onInteract.AddListener(MinhDontKnowPassword);
        Hung.anim.SetDirection(player.transform.position - Hung.transform.position);
    }
    void OnCommunicateWithNgan(InteractableEntity entity)
    {
        player.DisableMoveAndUI();
        GameManager.instance.dialogueManager.StartDialogue(texts.communicateWithNgan2, player.EnableMoveAndUI);
    }
    void OnCommunicateWithMinh(InteractableEntity entity)
    {
        player.DisableMoveAndUI();
        GameManager.instance.dialogueManager.StartDialogue(texts.communicateWithMinh2, player.EnableMoveAndUI);
    }
    void OnCommunicateWithMai(InteractableEntity entity)
    {
        player.DisableMoveAndUI();
        GameManager.instance.dialogueManager.StartDialogue(texts.communicateWithMai2, player.EnableMoveAndUI);
    }

    private void InitCharSecondMission(CharacterController character, Vector3 pos, Vector2 dir, Transform parent, UnityAction<InteractableEntity> onInteract = null)
    {
        character.SetPositon(pos, dir);
        character.transform.SetParent(parent, true);
        if (onInteract != null)
        {
            character.interact.onInteract.RemoveAllListeners();
            character.interact.canInteract = true;
            character.interact.onInteract.AddListener(onInteract);
        }
    }

    private void HungAskLinhAboutElectric()
    {
        player.DisableMoveAndUI();
        GameManager.instance.dialogueManager.StartDialogue(texts.HungAskPlayer, player.EnableMoveAndUI);
    }

    private void MinhDontKnowPassword(InteractableEntity minh)
    {
        player.DisableMoveAndUI();
        GameManager.instance.dialogueManager.StartDialogue(texts.askMinhAboutPassword, player.EnableMoveAndUI);
        Hung.interact.onInteract.RemoveAllListeners();
        Hung.interact.onInteract.AddListener(HungFindOutMorse);
    }

    private void HungFindOutMorse(InteractableEntity hung)
    {
        player.DisableMoveAndUI();
        GameManager.instance.dialogueManager.StartDialogue(texts.HungFindOutMorseCode, () =>
        {
            player.EnableMoveAndUI();
            GameManager.instance.missionsManager.AddAndShowMission(texts.findMorseCodeString);
        });
    }

    public void UnlockSmartElectric()
    {
        GameManager.instance.dialogueManager.StartDialogue(texts.HungThanksLinhForOpenningLock, EndDay3);
    }

    private void EndDay3()
    {
        GameManager.instance.transitions.Transition(1, 1, () =>
        {
            GameManager.instance.dialogueManager.StartDialogue(texts.endDayDialouge, MoveToDay4);
        }, InitAllCharMeetInLivingRoom);
    }

    private void MoveToDay4()
    {
        PlayerPrefs.SetInt("Progress", (int)GameProgress.CampingDay4);
        mainMapManager.InitCampingDay4();
        GameManager.instance.dbManager.UpdateDB();
    }
}
