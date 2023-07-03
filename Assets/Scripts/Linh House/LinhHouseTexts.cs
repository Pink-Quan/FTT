using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/In Linh house texts", fileName = "Language")]
public class LinhHouseTexts : ScriptableObject
{
    public Dialogue firstSeftDialogue;

    [TextArea]
    public string getCookBookGuide;
    public Dialogue seftDialogueAbountCookBook;

    public Dialogue monologueAbountLocker;

    [TextArea]
    public string getStick;
    public Dialogue monologueAbountStick;

    [TextArea]
    public string getLastPage;
    public Dialogue thereSomthingUnderTheChair;
    public Dialogue monologueAboutThePage;

    public Dialogue[] monologueAfterOpenningFacebook;
    public Dialogue monodialogueSomebodyKnockTheDoor;

    public Dialogue[] readingDocInLocker;

    public Dialogue monodialogueThereNoOneOutside;

    public Dialogue[] firstMeetNam;

    public Dialogue[] seeFoodInFirdge;

    public Dialogue[] LinhFeelingNotGood;

    public Dialogue NamTalkWithLinhWhenHerInBed;

    public Dialogue NamGuideLinhToBreath;
    public Dialogue NamTalkAfterLinhDoneBreath;

    public Dialogue[] LinhCommucatateThroughPhone;
}
