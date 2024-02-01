using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Language", menuName = "ScriptableObjects/Texts/Camping Day 3")]
public class CampingDay3Text : ScriptableObject
{
    public Dialogue[] wakePlayerUp;
    public Dialogue[] playerFirstDialgue;
    [TextArea]
    public string firstMissionText;
    public Dialogue monodialogueAboutDiryDisks;
    public string doneWashingDisksNotify;
    public string prepareFoodNotify;
    public string cookingFoodNotify;
    public string needPrepareFoodInFridgeFirst;
    public Dialogue forgotCheckDoor;
    public string lockDoor;
    public Dialogue checkWindow;
    public Dialogue monoDialogueAboutUnlockDoor;
    public Dialogue monoDialogueAboutPhone;
    public Dialogue monoDialogueAboutChess;
    public Dialogue fellDizzyAfterLookingAtChess;
    public Dialogue wakeUpAfterFainting;
    public Dialogue seeEveryoneDead;
    [TextArea] public string badEnding;
}
