using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HospitalManager : MonoBehaviour
{
    [SerializeField] private GameObject blackBackground;
    private HospitalConversation texts;

    [SerializeField] private Nurse nurse;
    public Transform guidenceNursePos;
    [SerializeField] private PlayerController player;

    [SerializeField] private HospitalCheckPoints checkpoints;
    [SerializeField] private ParticleSystem getCheckpointEff;

    [SerializeField] private GameObject closet;
    [SerializeField] private GameObject drinkMedacineTable;
    [SerializeField] private GameObject perscription;
    [SerializeField] private GameObject helper;
    private void Start()
    {
        texts = Resources.Load<HospitalConversation>($"Texts/Hospital/{PlayerPrefs.GetString("Language", "Eng")}");
        PlayFirstScene();
    }

    public void AddItemToPlayerInventory()
    {
        InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "Phone", 1, player.inventory);
        InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "Citizen Identity Card", 1, player.inventory);
    }

    private void PlayFirstScene()
    {
        GameManager.instance.dialogueManager.StartDialogue(texts.firstConversation, WakeUp);
    }

    private void WakeUp()
    {
        blackBackground.SetActive(false);
        GameManager.instance.dialogueManager.StartDialogue(texts.wakeUp, OnDoneWakeUpConversation);
    }

    private void OnDoneWakeUpConversation()
    {
        nurse.ReachPlayer(NuresComunitcateWithPlayer);
    }

    private void NuresComunitcateWithPlayer()
    {
        GameManager.instance.dialogueManager.StartDialogue(texts.talkingWithPlayerAffterWalkingUp, DoneTalkingWithPlayer);
    }

    public void DoneTalkingWithPlayer()
    {
        nurse.Disappeare(() =>
        {
            GameManager.instance.transitions.TransitionWithText(
            texts.afewDayLatter,
            () =>
            {
                nurse.transform.position = guidenceNursePos.position;
                player.transform.position = guidenceNursePos.position + Vector3.right;
                player.animator.enabled = true;
                player.transform.rotation = Quaternion.identity;
                player.col.enabled = true;
            },
            GuideHowToMoveAndTakeStuff);
        });
    }

    private void GuideHowToMoveAndTakeStuff()
    {
        nurse.transform.position = guidenceNursePos.position;
        GameManager.instance.dialogueManager.StartDialogue(texts.nurseShowPlayerHowToMove, () =>
        {
            GameManager.instance.textBoard.ShowText(texts.guideHowToMove, PlayerCheckpointsStart);
        });
    }

    private void PlayerCheckpointsStart()
    {
        player.playerMovement.enabled = true;
        checkpoints.gameObject.SetActive(true);
        player.SetArrowPointer(checkpoints.GetCheckPoints()[0]);

        checkpoints.OnDoneOneCheckPoint += player.SetArrowPointer;
        checkpoints.OnDoneOneCheckPoint += nextPoint => PlayGetCheckPointEffect();

        checkpoints.OnDoneAllCheckPoint += player.OffArrowPointer;
        checkpoints.OnDoneAllCheckPoint += PlayGetCheckPointEffect;
        checkpoints.OnDoneAllCheckPoint += TellPlayerDrinkDrug;

    }

    private void PlayGetCheckPointEffect()
    {
        getCheckpointEff.transform.position = player.transform.position;
        getCheckpointEff.Play();
    }

    private void TellPlayerDrinkDrug()
    {
        player.DisableMove();
        player.anim.SetMove(false);
        GameManager.instance.dialogueManager.StartDialogue(texts.nurseShowPlayerToTakeStuff, NurseLeaveRoom);
    }

    private void NurseLeaveRoom()
    {
        nurse.LeaveRoom(AllowPlayerExploreTheRoom);
    }

    private void AllowPlayerExploreTheRoom()
    {
        player.EnableMove();
        player.ShowButtons();
        closet.SetActive(true);
        drinkMedacineTable.SetActive(true);
        perscription.SetActive(true);
        player.buttons.SetActive(true);
    }

    public void ItemReceivedNotification()
    {
        ForbidPlayerMove();
        GameManager.instance.textBoard.ShowText(texts.getItemNotification, AllowPlayerMove);
    }

    public void AllowPlayerMove()
    {
        player.playerMovement.enabled = true;
        player.buttons.SetActive(true);
    }

    public void ForbidPlayerMove()
    {
        player.playerMovement.enabled = false;
        player.buttons.SetActive(false);
    }

    public void AddPrescriptionToPlayer(GameObject perscription)
    {
        if (!player.IsFlashLight())
        {
            GameManager.instance.dialogueManager.StartDialogue(texts.monoDialogueAbountSomethingUnderTheBed,AllowPlayerMove);
            return;
        }
        isSelfAsk = true;
        player.EnableMove();
        player.ShowButtons();
        GameManager.instance.fastNotification.Show(GameManager.instance.player.transform.position + Vector3.up * 0.5f, texts.getPrescriptionNofication);
        InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "How to take medicine", 1, player.inventory);
        perscription.SetActive(false);
    }

    bool isSelfAsk;
    public void AskSeftWhereIsPrescription()
    {
        if (!isSelfAsk)
        {
            ForbidPlayerMove();
            GameManager.instance.dialogueManager.StartDialogue(texts.seftAskWhereIsPrescription, null);
            isSelfAsk = true;
        }
    }

    public void TakeWrongDrug()
    {
        ForbidPlayerMove();
        GameManager.instance.dialogueManager.StartDialogue(texts.imFeelingNotGood, ()=> 
        {
            player.Die();
            PlayerPrefs.SetInt("FAINT IN HOSPITAL", 1);
            Invoke("LoseHospital", 2.5f);
        });
    }

    private void LoseHospital()
    {
        GameManager.instance.transitions.TransitionWithText(texts.lose, PlayAgain, null);
    }

    private void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TakeRightDrug()
    {
        ForbidPlayerMove();
        GameManager.instance.transitions.TransitionWithText(texts.afewDayLatter, MeetHelperAndGoHome, null);
    }

    private void MeetHelperAndGoHome()
    {
        helper.gameObject.SetActive(true);
        nurse.gameObject.SetActive(true);
        nurse.transform.position = guidenceNursePos.position;
        player.transform.position = guidenceNursePos.position + Vector3.right;
        player.anim.SetDirection(new Vector2(0, -1));
        player.TurnFlashLight(false);
        GameManager.instance.dialogueManager.StartDialogue(texts.goHome, ChangeToGoHomeScene);
    }

    private void ChangeToGoHomeScene()
    {
        GameManager.instance.transitions.Transition(1,1,null,()=> SceneManager.LoadScene("GoingHome"));
        PlayerPrefs.SetInt("Progress", (int)GameProgress.GoingHome);
        GameManager.instance.dbManager.UpdateDB();
    }

    bool isMonoDialougeAboutLocker;
    public void MonoDialogueAboutLocker()
    {
        if(isMonoDialougeAboutLocker) return;
        GameManager.instance.dialogueManager.StartDialogue(texts.monoDialgueAboutLocker, null);
        isMonoDialougeAboutLocker = true;
    }

    public void LockerHint()
    {
        GameManager.instance.textBoard.ShowText(texts.lockerHint);
    }

    public void MedicineHint()
    {
        GameManager.instance.textBoard.ShowText(texts.takeMedacineHint);
    }

}
