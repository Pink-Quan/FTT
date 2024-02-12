using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Language", menuName = "ScriptableObjects/Texts/Camping Day 4")]
public class CampingDay4Texts : ScriptableObject
{
    public Dialogue playerWakeUpDialogue;
    public Dialogue playerSeeHung;
    public Dialogue playerSeeEveryoneElse;
    public Dialogue[] playerFirstTalkWithNam;
    public string getFirstAidKit;
    public Dialogue[] talkWithNamAfterFirstAid;
    public Dialogue[] talkWithLinhInCliff;
    public Dialogue[] LinhShouldKnowTheTruth;
}
