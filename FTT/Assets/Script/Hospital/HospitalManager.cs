using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalManager : MonoBehaviour
{
    [SerializeField] private GameObject blackBackground;
    private HospitalConversation _conversation;

    [SerializeField] private CharactorController nurse;
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

    }    
}
