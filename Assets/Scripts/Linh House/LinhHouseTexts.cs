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
}
