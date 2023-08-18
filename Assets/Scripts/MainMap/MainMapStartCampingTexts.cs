using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Texts/MainMapStartCamping", fileName = "Language")]
public class MainMapStartCampingTexts : ScriptableObject
{
    public Dialogue[] theFirstConversationsWithManagers;

    [TextArea] public string firstMission;

    [Header("Start Camping Mission 1 Dialogue")]
    public Dialogue NamCamping1;
    public Dialogue MinhCamping1;
    public Dialogue NganCamping1;
    public Dialogue HungCamping1;
    public Dialogue MaiCamping1;
    public Dialogue cantDoFirstMission;
    public Dialogue needMagnet;
    public Dialogue[] MinhGuideToTakeMagnet;

    public Dialogue doneFirstMissons;
    public string getMagnet;

    [TextArea] public string callNganStartCampingMission;

    public Dialogue[] annouchToNganDoneFirstMission;

    [TextArea] public string scecondStartMission;

    [Header("Start camping second mission")]
    public Dialogue[] callNamSecondMission;
    public Dialogue[] callNganSecondMission;
    public Dialogue[] callMinhSecondMission;
    public Dialogue[] callHungSecondMission;
    public Dialogue[] callMaiSecondMission;

    public string getMagnetHint;
}
