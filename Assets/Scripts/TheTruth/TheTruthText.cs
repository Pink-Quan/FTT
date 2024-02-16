using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Language", menuName = "ScriptableObjects/Texts/TheTruth")]
public class TheTruthText : ScriptableObject
{
    [TextArea] public string news;
    public Dialogue HungFirstDialogue;
    public Dialogue HungSecondDialogue;
    public string fewMonthsAgo;

    [Header("A few months ago")]
    public Dialogue[] HungResolveTheQuestWithLinh;
    public Dialogue[] HungRememberThePast;
    public Dialogue HungAskWhereIsLinh;
    public Dialogue killerDialogue;
}
