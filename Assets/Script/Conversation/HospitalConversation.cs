using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Hospital conversation",fileName ="Language")]
public class HospitalConversation : ScriptableObject
{
    public Dialogue firstConversation;
    public Dialogue wakeUp;

    [Header("Nurse talking with player when walking up")]
    public Dialogue[] talkingWithPlayerAffterWalkingUp;

    public string afewDayLatter;

    public Dialogue nurseShowPlayerHowToMove;
    public Dialogue nurseShowPlayerToTakeStuff;

    public string guideHowToMove;
    public Dialogue afterDoneHowToMove;

    public string getItemNotification;
    public string getPrescriptionNofication;
    public Dialogue seftAskWhereIsPrescription;
    public Dialogue imFeelingNotGood;
}
