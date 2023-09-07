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

    public Dialogue[] callPlayerBackToCar;

    public Dialogue[] confessTheTruth;

}
