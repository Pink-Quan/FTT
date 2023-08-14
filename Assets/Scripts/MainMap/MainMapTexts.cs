using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Texts/MainMap", fileName = "Language")]
public class MainMapTexts : ScriptableObject
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

    [TextArea] public string scecondStartMission;
}
