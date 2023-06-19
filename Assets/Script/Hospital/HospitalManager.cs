using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalManager : MonoBehaviour
{
    [SerializeField] private GameObject blackBackground;
    private HospitalConversation _conversation;

    [SerializeField] private Nurse nurse;
    public Transform guidenceNursePos;
    [SerializeField] private PlayerController player;

    [SerializeField] private HospitalCheckPoints checkpoints;
    [SerializeField] private ParticleSystem getCheckpointEff;

    [SerializeField] private GameObject closet;
    [SerializeField] private GameObject drinkMedacineTable;
    [SerializeField] private GameObject perscription;
    private void Start()
    {
        _conversation = Resources.Load<HospitalConversation>($"Text/Hospital/{PlayerPrefs.GetString("Language","Viet")}");
        PlayFirstScene();
    }

    public void AddItemToPlayerInventory()
    {
        InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "Phone", 1, player.inventory);
        InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "Citizen Identity Card", 1, player.inventory);
    }

    private void PlayFirstScene()
    {
        GameManager.instance.dialogManager.StartDialogue(_conversation.firstConversation,WakeUp);
    }

    private void WakeUp()
    {
        blackBackground.SetActive(false);
        GameManager.instance.dialogManager.StartDialogue(_conversation.wakeUp, OnDoneWakeUpConversation);
    }  
    
    private void OnDoneWakeUpConversation()
    {
        nurse.ReachPlayer(NuresComunitcateWithPlayer);
    }    

    private void NuresComunitcateWithPlayer()
    {
        GameManager.instance.dialogManager.StartSequanceDialogue(_conversation.talkingWithPlayerAffterWalkingUp, DoneTalkingWithPlayer);
    }

    public void DoneTalkingWithPlayer()
    {
        nurse.Disappeare(()=> { GameManager.instance.transitions.TransitionWithText(
            _conversation.afewDayLatter,
            ()=> { 
                nurse.transform.position = guidenceNursePos.position;
                player.transform.position = guidenceNursePos.position + Vector3.right;
                player.animator.enabled = true;
                player.transform.rotation = Quaternion.identity;
                player.col.enabled = true;
            }, 
            GuideHowToMoveAndTakeStuff); });
    }

    private void GuideHowToMoveAndTakeStuff()
    {
        nurse.transform.position = guidenceNursePos.position;
        GameManager.instance.dialogManager.StartDialogue(_conversation.nurseShowPlayerHowToMove, ()=> 
        {
            GameManager.instance.textBoard.ShowText(_conversation.guideHowToMove, AllowMovePlayer);
        });
    }

    private void AllowMovePlayer()
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
        player.playerMovement.enabled=false;
        player.anim.SetMove(false);
        GameManager.instance.dialogManager.StartDialogue(_conversation.nurseShowPlayerToTakeStuff, NurseLeaveRoom);
    }

    private void NurseLeaveRoom()
    {
        nurse.LeaveRoom(AllowPlayerExploreTheRoom);
    }

    private void AllowPlayerExploreTheRoom()
    {
        player.playerMovement.enabled = true;
        closet.SetActive(true);
        drinkMedacineTable.SetActive(true);
        perscription.SetActive(true);
        player.buttons.SetActive(true);
    }

    public void ItemReceivedNotification()
    {
        ForbidPlayerMove();
        GameManager.instance.textBoard.ShowText(_conversation.getItemNotification,AllowPlayerMove);
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

    public void AddPerscriptionToPlayer()
    {
        InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "How to take medicine", 1, player.inventory);
    }
    
}
