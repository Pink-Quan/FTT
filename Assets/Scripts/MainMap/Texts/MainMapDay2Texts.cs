using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Language",menuName = "ScriptableObjects/Texts/MainMap Day 2")]
public class MainMapDay2Texts : ScriptableObject
{
    public Dialogue MinhFirstConversation;
    public Dialogue HungFirstConversation;
    public Dialogue NganFirstConversation;
    public Dialogue NamFirstConversation;
    [TextArea]
    public string firstMission;

    public Dialogue playerMonologueAboutFirstMisson;
    public Dialogue[] callPlayerBackToCar;
    public string backToCarParkMisson;

    public Dialogue[] confessTheTruth;
    public Dialogue NganGuidePlayerToDoMission3;
    [TextArea]
    public string mission3Description;

    public Dialogue[] talkToNganAboutNailInTree;
    public Dialogue playerMonodialogueCantTakeNail;

    public Dialogue[] anouchNganDoneMission3;
    public Dialogue[] endDay2Dialogue;
}
