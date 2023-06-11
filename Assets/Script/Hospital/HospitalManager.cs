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

    [SerializeField] private List<Transform> checkPoints;
    [SerializeField] private float checkPointRadius;

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
        StartCheckPoint();
        player.playerMovement.enabled = true;

    }

    private void StartCheckPoint()
    {
        StartCoroutine(CheckPlayerPassAllCheckPoints());
    }
    private IEnumerator CheckPlayerPassAllCheckPoints()
    {
        while (true)
        {
            foreach(var c in checkPoints.ToArray())
            {
                float sqrDis = (player.transform.position - c.position).sqrMagnitude;

                if (sqrDis < checkPointRadius * checkPointRadius)
                {
                    Debug.Log("Done check point");
                    Destroy(c.gameObject);
                    checkPoints.Remove(c);
                    if(checkPoints.Count==0)
                    {
                        DoneAllCheckPoints();
                        yield break;
                    }
                }
            }

            yield return null;
        }
    }

    private void DoneAllCheckPoints()
    {
        Debug.Log("Done ALL");
    }
}
