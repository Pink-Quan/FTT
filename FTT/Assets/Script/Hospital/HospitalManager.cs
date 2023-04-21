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
    private void Start()
    {
        _conversation = Resources.Load<HospitalConversation>($"Hospital/{PlayerPrefs.GetString("Language","Viet")}");
        PlayFirstScene();
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
            _conversation.afewDaylatter,
            ()=> { nurse.transform.position = guidenceNursePos.position;}, 
            GuideHowToMoveAndTakeStuff); });
    }

    private void GuideHowToMoveAndTakeStuff()
    {
        nurse.transform.position = guidenceNursePos.position;
        GameManager.instance.dialogManager.StartDialogue(_conversation.nurseShowPlayerHowToMove, AllowMovePlayer);
    }

    private void AllowMovePlayer()
    {
        Debug.Log("AllowMove");
    }
}
